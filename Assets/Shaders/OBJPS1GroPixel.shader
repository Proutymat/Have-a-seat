Shader "Custom/OBJPS1GroPixel"
{
	Properties
	{
		_Tint ("Tint", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_SnapResolution ("Snap Resolution", Range (60, 480.0)) =  148
		//_AffineWeight ("Affine Weight", Range(0.0, 1.0)) = 0.1
	}
	
    SubShader
    {
		Pass
		{
			CGPROGRAM
			
			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram
			#include "UnityCG.cginc"
			
			float4 _Tint;
			sampler2D _MainTex;
			float4 _MainTex_ST; //ST suffix stands for Scale and Translation
			float _SnapResolution, _AffineWeight;
			
			float clipW;
			float2 uvAffine;
			float2 uvPerspective;
			
			
			struct Interpolators
			{
				float4 position : SV_POSITION; //SV stands for system value, and POSITION for the final vertex position. 
				float2 uv : TEXCOORD0;
				float4 clipSpacePos : TEXCOORD1;
			};
			
			struct VertexData
			{
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
			};
			
			Interpolators MyVertexProgram (VertexData v) 
			{
				Interpolators i;
				i.uv = TRANSFORM_TEX(v.uv, _MainTex); //Simply form of v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
				
				float4 viewSpacePosition = mul(UNITY_MATRIX_MV, float4(v.position.xyz, 1.0));
				i.clipSpacePos = mul(UNITY_MATRIX_P, viewSpacePosition);
				
				float2 ndc = i.clipSpacePos.xy / i.clipSpacePos.w;
				ndc = round(ndc * _SnapResolution) / _SnapResolution;
				i.clipSpacePos.xy = ndc * i.clipSpacePos.w;
				
				i.uv *= i.clipSpacePos.w;
				
				i.position = i.clipSpacePos;
				
				return i;
			}

			float4 MyFragmentProgram (Interpolators i) : SV_TARGET
			{
				i.uv /= i.clipSpacePos.w;
				
				return tex2D(_MainTex, i.uv) * _Tint;
			}
			
			ENDCG
		}
	}
}

