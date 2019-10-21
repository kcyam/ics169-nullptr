Shader "Custom/CromadicWaterfallShader"
{
	Properties
	{
		_Color ( "Main Color", Color ) = ( 1.0, 1.0, 1.0, 1.0 )
		_WaveColor ( "Wave Color", Color ) = ( 1.0, 1.0, 1.0, 1.0 )
		_Bands ( "Cel Bands", Range(2,10) ) = 2
		_WaveAmplitude ( "Wave Amplitude", Float ) = 0.1
		_Noise ( "Noise Texture", 2D ) = "white" {}
	}
	
    SubShader {
        Tags
		{
			"RenderType" = "Opaque"
		}
		
		CGPROGRAM
		#include "AutoLight.cginc"
		#pragma surface surf Ramp vertex:vert finalcolor:mycolor
		
		//Properties - grabbed from Properties block above
		float4 	_Color;
		float4	_WaveColor;
		int 	_Bands;
		float	_WaveAmplitude;
		sampler2D _Noise;
		
		struct SurfaceOutputCustom
		{
			fixed3 Albedo;
			fixed3 Normal;
			fixed3 Emission;
			half Specular;
			fixed Gloss;
			fixed Alpha;
			
			float4 vertex : POSITION;
		};
		
		half4 LightingRamp (SurfaceOutputCustom s, half3 lightDir, half atten) {
			
			half4 c;
			
			half4 shadowColor = half4( 0.05,0,0.05,1 );
			half dp = dot (s.Normal, lightDir);
			
			if( atten > 0.1 ){
				atten = atten * 2;
			}
			else{
				atten = 0;
				c = half4(0,0,0,1);
				return c;
			}
			
			dp = dp + 1.0 / 2.0;
			int bandOn =  dp * _Bands ;
			bandOn = _Bands - bandOn;
			
			if(bandOn > 0.4 * _Bands){
				//atten = 0;
				//c = shadowColor;
				//return c;
				bandOn = _Bands-2;
			}
			
			float4 newCol = _Color;
			
			newCol = ( newCol + _LightColor0 ) / 2;
			//newCol = _Color;
			
			float4 diff = newCol - shadowColor;
			newCol = newCol - ( diff * bandOn / ( _Bands - 1 ) );
			
			//c.rgb = max(newCol.rgb * atten, newCol.rgb * shadowAtten);
			c.rgb = newCol.rgb * atten;
			
			c.a = s.Alpha;
			
			return c;
		}
		
		struct Input{
			float4 color : COLOR;
			float3 worldNormal;
			float3 localPos;
			float extrusion;
			INTERNAL_DATA
		};
		
		void mycolor(Input IN, SurfaceOutputCustom o, inout fixed4 color){
		
		}
		
		void vert( inout appdata_full v, out Input o ){
			float4 newVertexPos = mul( unity_ObjectToWorld, v.vertex );
			
			//float wavePos = 3 - ( _Time.r % 3.5 );
			//float extrusion = pow( max( 0, -pow( (v.vertex.g+1) - wavePos, 2 ) + 1.0 ), 30);
			
			float extrusion = max( sin(_Time.r *200 + v.vertex.g*50), 0);
			
			newVertexPos = newVertexPos + float4(extrusion * v.normal * _WaveAmplitude, 0);
			
			newVertexPos.g += tex2Dlod(_Noise, normalize(float4((v.vertex.r + 1) / 2, (v.vertex.b + 1 ) / 2, 0, 0)));
			
			v.vertex = mul( unity_WorldToObject, newVertexPos );
			UNITY_INITIALIZE_OUTPUT(Input,o);
			o.extrusion = extrusion;
		}
		
		void surf( Input IN, inout SurfaceOutputCustom o ){
			//float3 localPos = IN.worldPos -  mul(unity_ObjectToWorld, float4(0,0,0,1)).xyz;
			
			//float wavePos = 3 - ( _Time.a % 3.5 );
			//float extrusion = pow( max( 0, -pow( (IN.localPos.g+1) - wavePos, 2 ) + 1.0 ), 30);
			
			if(IN.extrusion > 0.7)
			{
				o.Albedo = _WaveColor;
			}
			else
			{
				o.Albedo = half3(0,0,0);
			}
		}
		
		ENDCG
    }
}