Shader "Custom/CromadicFireShader"
{
	Properties
	{
		_Color ( "Main Color", Color ) = ( 1.0, 1.0, 1.0, 1.0 )
		_LightDir ( "Light Direction", Vector ) = ( 0, -1, 0 )
		_Bands ( "Cel Bands", Range(2,10) ) = 2
		_ShadowColor ( "Shadow Color", Color ) = ( 0,0,0,1 )
		_SpecularExponent ( "Specular Exponent", Float ) = 1
		_SpecularColor ( "Specular Color", Color ) = ( 1, 1, 1, 1 )
	}
	
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
				o.pos = UnityObjectToClipPos(v.vertex);
				
				//Calculate normal in world space
				float4 normal4 = float4(v.normal, 0.0);
				o.normal = normalize( mul( normal4, unity_WorldToObject ).xyz );
				
				o.viewDir = normalize( _WorldSpaceCameraPos - mul( unity_ObjectToWorld, v.vertex ).xyz );
				
				return o;
			}
			
			//FRAGMENT SHADER
			float4 frag( vertOutput input ) : COLOR
			{
				
				float3 lightDir = normalize(-input.viewDir);
				float dp = dot( lightDir, input.normal );
				int bandOn =  ( dp * -dp + 1 ) / 2 * _Bands ;
				
				float4 diff = _Color - _ShadowColor;
				float4 newCol = _Color - ( diff * bandOn / ( _Bands - 1 ) );
				
				return float4( newCol.xyz, 1 );
			}
			
			ENDCG
        }
    }
}
