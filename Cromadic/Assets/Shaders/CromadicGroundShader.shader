Shader "Custom/CromadicGroundShader"
{
	Properties
	{
		_Color ( "Main Color", Color ) = ( 1.0, 1.0, 1.0, 1.0 )
		_TopColor ( "Top Color", Color ) = ( 1.0, 1.0, 1.0, 1.0 )
		_BottomColor ( "Bottom Color", Color ) = ( 1.0, 1.0, 1.0, 1.0 )
		_Bands ( "Cel Bands", Range(2,10) ) = 2
		_SpecularExponent ( "Specular Exponent", Float ) = 1
		_SpecularColor ( "Specular Color", Color ) = ( 1, 1, 1, 1 )
		_Seed ( "Seed", Float ) = 0.8
		_Jaggedness ( "Jaggedness", Float ) = 0.1
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
		float4	_TopColor;
		float4	_BottomColor;
		float3 	_LightDir;
		int 	_Bands;
		float	_SpecularExponent;
		float4	_SpecularColor;
		float	_Seed;
		float 	_Jaggedness;
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
			if( s.Normal.g > 0.5 ){
				newCol = _TopColor;
			}
			
			else if( s.Normal.g < -0.5 ){
				newCol = _BottomColor;
			}
			
			else{
				newCol = _Color;
			}
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
			float4 newVertexPos = mul( unity_ObjectToWorld, v.vertex );
				
			float tX = (newVertexPos.x * newVertexPos.x * _Seed) % 1;
			float tY = (newVertexPos.y * newVertexPos.y * _Seed) % 1;
			float tZ = (newVertexPos.z * newVertexPos.x * _Seed) % 1;
			float noiseAddX = (tex2Dlod(_Noise, normalize(float4(tX, tY, 0, 0))) * 2 - 1) * _Jaggedness;
			float noiseAddY = (tex2Dlod(_Noise, normalize(float4(tY, tZ, 0, 0))) * 2 - 1) * _Jaggedness;
			float noiseAddZ = (tex2Dlod(_Noise, normalize(float4(tZ, tX, 0, 0))) * 2 - 1) * _Jaggedness;
			
			newVertexPos = float4( newVertexPos.x + noiseAddX, newVertexPos.y + noiseAddY, newVertexPos.z + noiseAddZ, newVertexPos.w );
			
			//Calculate position in camera clip space
			v.vertex = mul( unity_WorldToObject, newVertexPos );
		}
		
		void surf( Input IN, inout SurfaceOutputCustom o ){
			//*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*
			//*  NOTE: IF YOU WANT TO VIEW  *
			//*THE ENTIRE LEVEL MORE EASILY,*
			//*  CHANGE "FALSE" TO "TRUE"   *
			//*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*
			
			if(false)
			{
				if(o.Normal.g > 0.5){
					o.Albedo = _TopColor;
				}
				else{
					o.Albedo = _Color;
				}
				return;
			}
			o.Albedo = half3(0,0,0);
		}
		
		ENDCG
    }
}