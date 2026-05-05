Shader "Unlit/PostProcessGroPixel"
{
    Properties
    {
        [NoScaleOffset] [MainTexture] _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram
            #include "UnityCG.cginc"
            
            sampler2D _MainTex;
            half4x4 _Ps1Dither = half4x4(
				-4.0, 0.0, -3.0, 1.0,
				2.0, -2.0, 3.0, -1.0,
				-3.0, 1.0, -4.0, 0.0,
				3.0, -1.0, 2.0, -2.0
            );
            
            
            struct Interpolators
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uvDetail : TEXCOORD1;
			};
			
			struct VertexData
			{
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
			};
			
			Interpolators MyVertexProgram (VertexData v) 
			{
				Interpolators i;
				i.position = UnityObjectToClipPos(v.position);
				i.uv = v.uv;
				return i;
			}

			float4 MyFragmentProgram (Interpolators i) : SV_TARGET
			{
				float3 color = tex2D(_MainTex, i.uv).rgb;
				half4x4 _Ps1Dither = {
					-4.0, 0.0, -3.0, 1.0,
					2.0, -2.0, 3.0, -1.0,
					-3.0, 1.0, -4.0, 0.0,
					3.0, -1.0, 2.0, -2.0
				};
				
				float2 realResolution = float2(480, 270);
				float2 virtualResolution = float2(320, 180);

				float2 virtualPos = floor(i.position.xy * (realResolution / virtualResolution));
				int x = int(virtualPos.x) % 4.0;
				int y = int(virtualPos.y) % 4.0;
				int index = (y + x);
				
				float dither_offset = (_Ps1Dither[index]/8.0);
				
				float3 quantizedColor = floor((color * 255.0) / 8.0 + dither_offset) / 31.0;
				
				return float4(quantizedColor, 1);
			}
            ENDCG
        }
    }
}
