Shader "Custom/CromadicWater"
{
	Properties
	{
		_Color ( "Main Color", Color ) = ( 1.0, 1.0, 1.0, 1.0 )
		_Bands ( "Cel Bands", Range(2,10) ) = 2
		_SpecularExponent ( "Specular Exponent", Float ) = 1
		_SpecularColor ( "Specular Color", Color ) = ( 1, 1, 1, 1 )
		_AbsoluteHeight ( "Aboslute Height", Float ) = 1
		_Noise ( "Noise Texture", 2D ) = "white" {}
		_WaveSpeed ( "Wave Speed", Float ) = 1
		_WaveHeight ( "Wave Height", Float ) = 1
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
		int 	_Bands;
		float4 	_ShadowColor;
		float	_SpecularExponent;
		float4	_SpecularColor;
		float	_AbsoluteHeight;
		float	_WaveSpeed;
		float	_WaveHeight;
		sampler2D	_Noise;
			
		struct SurfaceOutputCustom
		{
			fixed3 Albedo;
			fixed3 Normal;
			fixed3 Emission;
			half Specular;
			fixed Gloss;
			fixed Alpha;
			
			fixed edge;
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
			
			float4 newCol;
			newCol = _Color;
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
			INTERNAL_DATA
		};
		
		void mycolor(Input IN, SurfaceOutputCustom o, inout fixed4 color){
			//color = pow(color, 1.5);
		}
		
		void vert( inout appdata_full v ){
				
			//Calculate position in camera clip space
			if(v.vertex.y > _AbsoluteHeight)
			{
				v.vertex = float4( v.vertex.x, _AbsoluteHeight, v.vertex.z, v.vertex.w );
			}
			float4 newVertexPos = mul( unity_ObjectToWorld, v.vertex );
			
			float4 newNorm = newVertexPos;
			
			//Add water noise
			float noiseAdd = tex2Dlod(_Noise, normalize(float4(v.texcoord.xy, 0, 0)));
			newVertexPos.y += sin((_Time + 1) * noiseAdd * _WaveSpeed ) * _WaveHeight;
			newVertexPos.x += cos((_Time + 1) * noiseAdd * _WaveSpeed ) * _WaveHeight;
			
			//Calculate normal in world space
			newNorm = normalize(newVertexPos - newNorm);
			newNorm.b = abs(newNorm.b);
			v.normal = newNorm;
			//float4 normal4 = float4(v.normal, 0.0);
			//o.normal = normalize( mul( normal4, unity_WorldToObject ).xyz );
			
			//o.viewDir = normalize( _WorldSpaceCameraPos - mul( unity_ObjectToWorld, v.vertex ).xyz );
			
			//Calculate position in camera clip space
			v.vertex = mul( unity_WorldToObject, newVertexPos );
		}
		
		void surf( Input IN, inout SurfaceOutputCustom o ){
			o.Albedo = half3(0,0,0);
			o.Normal = WorldNormalVector ( IN, float3( 0, 0, 1 ) );
		}
		
		ENDCG
    }
	/*
    SubShader {
        Pass {
            Tags
            {
                "Queue" = "Geometry"
            }
			
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			//Properties - grabbed from Properties block above
			uniform float4 	_Color;
			uniform float3 	_LightDir;
			uniform int 	_Bands;
			uniform float4 	_ShadowColor;
			uniform float	_SpecularExponent;
			uniform float4	_SpecularColor;
			uniform float	_AbsoluteHeight;
			uniform float	_WaveSpeed;
			uniform float	_WaveHeight;
			
			uniform sampler2D _Noise;
			
			//For the vertex shader
			struct vertInput
			{
				float4 vertex	:	POSITION;
				float3 normal	:	NORMAL;
				float4 texCoord	:	TEXCOORD0;
			};
			
			struct vertOutput
			{
				float4 pos		:	SV_POSITION;
				float3 normal	:	NORMAL;
				float3 viewDir	:	TEXCOORD1;
				float3 texCoord	:	TEXCOORD0;
				fixed3 color	:	COLOR0;
			};
			
			
			//VERTEX SHADER
			vertOutput vert( vertInput v )
			{
				vertOutput o;
				
				//Calculate position in camera clip space
				float4 newPos;
				if(v.vertex.y > _AbsoluteHeight)
				{
					v.vertex = float4( v.vertex.x, _AbsoluteHeight, v.vertex.z, v.vertex.w );
				}
				o.pos = UnityObjectToClipPos( v.vertex );
				float4 newNorm = o.pos;
				
				//Add water noise
				float noiseAdd = tex2Dlod(_Noise, normalize(float4(v.texCoord.xy, 0, 0)));
				o.pos.y += sin((_Time + 1) * noiseAdd * _WaveSpeed ) * _WaveHeight;
				o.pos.x += cos((_Time + 1) * noiseAdd * _WaveSpeed ) * _WaveHeight;
				
				//Calculate normal in world space
				newNorm = normalize(o.pos - newNorm);
				o. normal = newNorm;
				//float4 normal4 = float4(v.normal, 0.0);
				//o.normal = normalize( mul( normal4, unity_WorldToObject ).xyz );
				
				o.viewDir = normalize( _WorldSpaceCameraPos - mul( unity_ObjectToWorld, v.vertex ).xyz );
				
				return o;
			}
			
			//FRAGMENT SHADER
			float4 frag( vertOutput input ) : COLOR
			{
				float3 lightDir = normalize(_LightDir);
				float dp = dot( lightDir, input.normal );
				int bandOn =  ( dp + 1 ) / 2 * _Bands ;
				
				float4 diff = _Color - _ShadowColor;
				float4 newCol = _Color - ( diff * bandOn / ( _Bands - 1 ) );
				
				//Specular highlights
				dp = pow( dot( input.viewDir, reflect( lightDir, input.normal ) ), _SpecularExponent );
				if( dp > 0.5 )
				{
					newCol = newCol + _SpecularColor;
				}
				
				return float4( newCol.xyz, 1 );
			}
			
			ENDCG
			
        }
    }
	*/
}
