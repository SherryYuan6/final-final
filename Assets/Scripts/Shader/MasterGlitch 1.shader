Shader "Custom/UI/BlackGlitchOverlay"
{
    Properties
    {
        _Tint ("Tint", Color) = (0,0,0,1)
        _Intensity ("Intensity", Range(0,1)) = 0
        _BlockSize ("Block Size", Range(5,200)) = 60
        _Speed ("Speed", Range(0,20)) = 8
        _DistortionStrength ("Distortion", Range(0,0.1)) = 0.03
    }

    SubShader
    {
        Tags
        {
            "Queue"="Overlay"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }

        Pass
        {
            Blend One Zero
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _Tint;
                float _Intensity;
                float _BlockSize;
                float _Speed;
                float _DistortionStrength;
            CBUFFER_END

            float hash21(float2 p)
            {
                p = frac(p * float2(123.34, 456.21));
                p += dot(p, p + 34.345);
                return frac(p.x * p.y);
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;
                float time = _Time.y * _Speed;

                // --- BLOCK GRID ---
                float blockY = floor(uv.y * _BlockSize);
                float blockX = floor(uv.x * (_BlockSize * 0.5));

                float blockNoise = hash21(float2(blockX, blockY + floor(time)));

                // full corruption regions (NO transparency)
                float corruption = step(0.65, blockNoise);

                // --- HORIZONTAL TEARING ---
                float tearLine = step(0.92, hash21(float2(blockY, floor(time * 3.0))));
                float offsetX = (hash21(float2(blockY, time)) - 0.5) * _DistortionStrength * tearLine;

                uv.x += offsetX;

                // --- SCANLINE DARKENING (not transparency) ---
                float scan = sin(uv.y * 600.0 + time * 5.0) * 0.5 + 0.5;
                float scanMask = lerp(0.6, 1.0, scan);

                // --- FINAL MASK ---
                float mask = max(corruption, tearLine);

                // force full coverage
                float3 col = _Tint.rgb;

                // dark glitch blocks
                col *= lerp(0.2, 1.0, mask);

                // scanline interference
                col *= scanMask;

                // intensity drives visibility loss, not transparency
                col = lerp(col, float3(0,0,0), _Intensity);

                return float4(col, 1.0);
            }
            ENDHLSL
        }
    }
}