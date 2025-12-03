Shader "Custom/UnlitScentTrail_ArrowSnake"
{
    Properties
    {
        _MainTex ("Grayscale Texture", 2D) = "white" {}
        _Color ("Trail Color", Color) = (0.5, 1, 0.5, 0.7)
        _Distortion ("Wave Distortion", Range(0, 0.5)) = 0.15
        _Speed ("Wave Speed", Range(0.1, 5)) = 1.5
        _Frequency ("Wave Frequency", Range(1, 20)) = 6.0
        _BloomIntensity ("Bloom Intensity", Range(1, 5)) = 2.0
        _TipFade ("Tip Fade Strength", Range(0, 1)) = 0.3
        _DashDensity ("Arrow Density", Range(1, 20)) = 6.0
        _FlowSpeed ("Flow Speed", Range(0.1, 5)) = 1.2
        _Sharpness ("Arrow Sharpness", Range(0.1, 10)) = 4.0
        _SideFade ("Side Fade", Range(0, 0.5)) = 0.1

        // === NEW ===
        _OverallAlpha ("Overall Transparency", Range(0, 1)) = 1.0
        _AlphaPulseSpeed ("Alpha Pulse Speed", Range(0, 10)) = 0.0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Distortion;
            float _Speed;
            float _Frequency;
            float _BloomIntensity;
            float _TipFade;
            float _DashDensity;
            float _FlowSpeed;
            float _Sharpness;
            float _SideFade;

            // === NEW ===
            float _OverallAlpha;
            float _AlphaPulseSpeed;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t = _Time.y * _Speed;

                // === Snake-like wiggle ===
                float wave = sin(i.uv.y * _Frequency + t) * _Distortion;
                float2 wavedUV = i.uv + float2(wave, 0);
                wavedUV.x = saturate(wavedUV.x);

                // === Arrow pattern ===
                float flow = frac(i.uv.y * _DashDensity - _Time.y * _FlowSpeed);
                float tri = abs(frac(flow + 0.5) * 2.0 - 1.0);
                tri = pow(1.0 - tri, _Sharpness);
                float arrowShape = tri * (1.0 - abs(wavedUV.x * 2.0 - 1.0));

                // === Sample texture ===
                float baseTex = tex2D(_MainTex, wavedUV).r;
                float intensity = baseTex * arrowShape;

                // === Color & alpha ===
                fixed4 col = _Color;
                col.rgb *= intensity;
                col.a *= intensity;

                // === Fade top/bottom ===
                float bottomFade = smoothstep(0.0, 0.1, i.uv.y);
                float topFade = 1.0 - smoothstep(0.9, 1.0, i.uv.y);
                col.a *= lerp(bottomFade, bottomFade * topFade, _TipFade);

                // === Side fade ===
                float sideFade = smoothstep(0.0, _SideFade, wavedUV.x) * (1.0 - smoothstep(1.0 - _SideFade, 1.0, wavedUV.x));
                col.a *= sideFade;

                // === Bloom ===
                col.rgb *= _BloomIntensity;

                // === Overall Transparency (static + optional pulse) ===
                float pulse = 1.0;
                if (_AlphaPulseSpeed > 0.0)
                    pulse = 0.5 + 0.5 * sin(_Time.y * _AlphaPulseSpeed); // oscillates 0–1

                col.a *= _OverallAlpha * pulse;

                return col;
            }
            ENDCG
        }
    }
    FallBack Off
}
