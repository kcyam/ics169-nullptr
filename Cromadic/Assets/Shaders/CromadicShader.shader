Shader "Custom/CromadicShader"
{
	Properties
	{
		_Color ( "Main Color", Color ) = ( 1.0, 1.0, 1.0, 1.0 )
		_LightDir ( "Light Direction", Vector ) = ( 0, -1, 0 )
		_Bands ( "Cel Bands", Range(2,10) ) = 2
		_SpecularExponent ( "Specular Exponent", Float ) = 1
		_SpecularColor ( "Specular Color", Color ) = ( 1, 1, 1, 1 )
	}
	
    SubShader {
        Tags
		{
			"RenderType" = "Opaque"
		}
		CGPROGRAM
		#include "AutoLight.cginc"
		#pragma surface surf Ramp vertex:vert fullforwardshadows
		
		//Properties - grabbed from Properties block above
		float4 	_Color;
		float3 	_LightDir;
		int 	_Bands;
		float	_SpecularExponent;
		float4	_SpecularColor;
		
		half4 LightingRamp (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
			if( dot( viewDir, s.Normal ) < 0.4 )
			{
				//return half4( 0.05,0,0.05,1 );
			}
			
			half4 c;
			
			half4 shadowColor = half4( 0.05,0,0.05,1 );
			half dp = dot (s.Normal, lightDir);
			
			if( atten > 0.08 ){
				atten = atten * 2;
			}
			else{
				atten = 0;
				c = half4(0,0,0,1);
				return c;
			}
			
			int bandOn =  dp * _Bands ;
			bandOn = _Bands - bandOn;
			
			float4 newCol = ( _Color + _LightColor0 ) / 2;
			//newCol = _Color;
			
			float4 diff = newCol - shadowColor;
			newCol = newCol - ( diff * bandOn / ( _Bands - 0 ) );
			
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
		
		void vert( inout appdata_full v ){
			
		}
		
		void surf( Input IN, inout SurfaceOutput o ){
			//*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*
			//*  NOTE: IF YOU WANT TO VIEW  *
			//*THE ENTIRE LEVEL MORE EASILY,*
			//*  CHANGE "FALSE" TO "TRUE"   *
			//*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*
			
			if(false)
			{
				o.Albedo = _Color/12;
				return;
			}
			o.Albedo = half3(0,0,0);
		}
		
		/*
		//VERTEX SHADER
		vertOutput vert( vertInput v )
		{
			vertOutput o;
			
			//Calculate position in camera clip space
			o.pos = UnityObjectToClipPos(v.vertex);
			
			//Calculate color for this vertex
			//o.color = v.normal * 0.5 + 0.5;
			
			//Calculate normal in world space
			float4 normal4 = float4(v.normal, 0.0);
			o.normal = normalize( mul( normal4, unity_WorldToObject ).xyz );
			
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
		*/
		ENDCG
    }
}