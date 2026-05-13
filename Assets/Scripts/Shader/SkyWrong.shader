Shader "Collapse/SkyWrong"
{
    Properties
    {
        _SkyDegradation ("Sky Degradation", Range(0,1)) = 0
        _SkyColorA ("Sky Top", Color) = (0.45, 0.60, 0.90, 1)
        _SkyColorB ("Sky Horizon", Color) = (0.70, 0.85, 1.0, 1)
        _GroundColor ("Ground Color", Color) = (0.25, 0.22, 0.20, 1)
        _SunColor ("Sun Color Normal", Color) = (1.0, 0.95, 0.70, 1)
        _SunColorWrong ("Sun Color Wrong", Color) = (0.90, 0.30, 0.60, 1)
        _SunSize ("Sun Size", Range(0.01, 0.2)) = 0.06
    }

    SubShader
    {
        Tags { "RenderType"="Background" "Queue"="Background" }
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 worldPos   : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            float _SkyDegradation;
            float _ChipTime;
            float4 _SkyColorA;
            float4 _SkyColorB;
            float4 _GroundColor;
            float4 _SunColor;
            float4 _SunColorWrong;
            float  _SunSize;

            float hash(float2 p) { return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453); }
            float noise(float2 p)
            {
                float2 i = floor(p); float2 f = frac(p);
                float a = hash(i), b = hash(i+float2(1,0)), c = hash(i+float2(0,1)), d = hash(i+float2(1,1));
                float2 u = f*f*(3.0-2.0*f);
                return lerp(lerp(a,b,u.x),lerp(c,d,u.x),u.y);
            }

            Varyings Vert(Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.worldPos   = IN.positionOS.xyz;
                return OUT;
            }

            half4 Frag(Varyings IN) : SV_Target
            {
                float3 dir = normalize(IN.worldPos);
                float  t   = _ChipTime;
                float  deg = _SkyDegradation;

                float sky = saturate(dir.y * 2.0 + 0.1);
                half3 skyCol = lerp(_SkyColorB.rgb, _SkyColorA.rgb, sky);

                float hueShift = noise(dir.xz * 6.0 + t * 0.2) * deg;
                half3 wrongSky = skyCol.brg;
                skyCol = lerp(skyCol, wrongSky, hueShift * 0.7);

                half3 groundCol = _GroundColor.rgb;
                half3 col = lerp(groundCol, skyCol, saturate(dir.y * 3.0 + 0.3));

                float2 sunPos = float2(0.6 + sin(t * 0.04) * 0.1 * deg, 0.65 + noise(float2(t * 0.05, 0)) * 0.1 * deg);
                float2 uvSky  = float2(atan2(dir.x, dir.z) / (2.0 * 3.14159) + 0.5, dir.y * 0.5 + 0.5);
                float  sunDist = length(uvSky - sunPos);
                half3  sunCol  = lerp(_SunColor.rgb, _SunColorWrong.rgb, deg * 0.8);
                float  sunMask = 1.0 - smoothstep(_SunSize * 0.5, _SunSize, sunDist);
                col = lerp(col, sunCol, sunMask * 0.9);

                float lightWrong = noise(float2(dir.x * 2.0, t * 0.1)) * deg * 0.3;
                col += half3(-lightWrong * 0.1, lightWrong * 0.05, lightWrong * 0.15);

                return half4(saturate(col), 1.0);
            }
            ENDHLSL
        }
    }
}