Shader "HaveASeat/OBJPS1GroPixelLit"
{
	Properties
	{
		_Tint ("Tint", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		[Gamma] _Metallic ("Metallic", Range(0,1)) = 0
		_Smoothness ("Smoothness", Range(0,1)) = 0.5
		_SnapResolution ("Snap Resolution", Range (60, 480.0)) =  148
		//_AffineWeight ("Affine Weight", Range(0.0, 1.0)) = 0.1
	}
	
    SubShader
    {
	    Tags 
        { 
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalRenderPipeline"
        }
    	        
		Pass
		{
			Tags
			{
				"LightMode" = "UniversalForward"
			}
			
			HLSLPROGRAM
			
			#define _SPECULAR_COLOR_SPECULAR_COLOR
			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram
			#pragma multi_compile _ _ADDITIONAL_LIGHTS
			#pragma multi_compile_fragment _ _SHADOWS_SOFT
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _FORWARD
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_SCREEN

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			
			float4 _Tint;
			sampler2D _MainTex;
			float4 _MainTex_ST; //ST suffix stands for Scale and Translation
			float _SnapResolution, _AffineWeight, _Smoothness, _Metallic;
			
			float clipW;
			float2 uvAffine;
			float2 uvPerspective;
			
			
			struct Interpolators
			{
				float4 position : SV_POSITION; //SV stands for system value, and POSITION for the final vertex position. 
				float2 uv : TEXCOORD0;
				float4 clipSpacePos : TEXCOORD1;
				float3 normal : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
			};
			
			struct VertexData
			{
				float4 position : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};
			
			Interpolators MyVertexProgram (VertexData v) 
			{
				Interpolators i;
				i.uv = TRANSFORM_TEX(v.uv, _MainTex); //Simply form of v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
				
				i.worldPos = mul(unity_ObjectToWorld, v.position).xyz;
				i.normal = TransformObjectToWorldNormal(v.normal);
				
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
    
			    i.normal = normalize(i.normal);
			    float4 texColor = tex2D(_MainTex, i.uv) * _Tint;
			    float3 albedo = texColor.rgb;
			    
				InputData inputData = (InputData)0;
				inputData.positionWS = i.worldPos;
				inputData.normalWS = normalize(i.normal);
				inputData.viewDirectionWS = GetWorldSpaceNormalizeViewDir(i.worldPos);
				inputData.shadowMask = half4(1,1,1,1);
				inputData.shadowCoord = TransformWorldToShadowCoord(i.worldPos);

				float3 lighting = albedo * 0.05;

				// Main light
				Light mainLight = GetMainLight(inputData.shadowCoord, inputData.positionWS, inputData.shadowMask);
				lighting += LightingLambert(mainLight.color,mainLight.direction, inputData.normalWS) * mainLight.shadowAttenuation;

				// Additional lights
				uint lightCount = GetAdditionalLightsCount();

				for (uint iLight = 0; iLight < lightCount; ++iLight)
				{
				    Light light = GetAdditionalLight(iLight, inputData.positionWS,half4(1,1,1,1));
				    lighting += LightingLambert(light.color, light.direction, inputData.normalWS) * light.distanceAttenuation * light.shadowAttenuation;
				}
				
				return float4(lighting * albedo, 1);
			}
			ENDHLSL
       }
		UsePass "Universal Render Pipeline/Simple Lit/ShadowCaster"
	}
}

