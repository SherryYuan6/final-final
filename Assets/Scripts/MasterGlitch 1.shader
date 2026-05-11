Shader "Custom/UI/BlackGlitchOverlay"
{
    Properties
    {
        _Tint ("Tint", Color) = (0,0,0,1)
        _Intensity ("Intensity", Range(0,1)) = 0
        _BlockSize ("Block Size", Range(1,100)) = 30
        _Speed ("Speed", Range(0,20)) = 8
        _ScanlineStrength ("Scanline Strength", Range(0,2)) = 0.3
        _Flicker ("Flicker", Range(0,1)) = 0.15
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "RenderPipeline"="UniversalPipeline"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

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
                float _ScanlineStrength;
                float _Flicker;
            CBUFFER_END

            float hash21(float2 p)
            {
                p = frac(p * float2(123.34, 456.21));
                p += dot(p, p + 45.32);
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

                float bandY = floor(uv.y * _BlockSize);
                float bandNoise = hash21(float2(bandY, floor(time * 8.0)));

                float glitchMask = step(0.72, bandNoise);

                float lineNoise = hash21(float2(floor(uv.y * 200.0), floor(time * 25.0)));
                float thinLines = step(0.92, lineNoise);

                float scan = sin(uv.y * 700.0) * 0.5 + 0.5;
                float flicker = 1.0 - (_Flicker * hash21(float2(floor(time * 18.0), 2.31)));

                float alpha = 0.0;

                alpha += glitchMask * (0.15 + 0.35 * _Intensity);
                alpha += thinLines * (0.08 + 0.2 * _Intensity);
                alpha += (1.0 - scan) * _ScanlineStrength * 0.08 * _Intensity;

                alpha *= flicker;
                alpha = saturate(alpha * _Intensity);

                half4 col = _Tint;
                col.a = alpha;

                return col;
            }
            ENDHLSL
        }
    }
}