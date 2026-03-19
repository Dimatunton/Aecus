Shader "Custom/SmellTrail/Tail"
{
    Properties
    {
        _MainTex ("Main Texture (RGBA)", 2D) = "white" {}
        _NoiseTex ("Noise (R)", 2D) = "white" {}
        _Color ("Tint", Color) = (1,0.6,0.2,1)
        _NoiseScale ("Noise Scale", Float) = 1.0
        _NoiseStrength ("Noise Strength", Range(0,2)) = 0.6
        _Speed ("Flow Speed", Float) = 1.0
        _AlphaFalloff ("Alpha Falloff", Range(0,4)) = 1.5
        _EdgeSoftness ("Edge Softness", Range(0,1)) = 0.2

        // Sway controls
        _SwayAmount ("Sway Amount (world)", Float) = 0.5
        _SwayFrequency ("Sway Frequency", Float) = 1.0
        _SwayFalloff ("Sway Falloff", Float) = 1.8

        // Stretching control for scaled quads: multiply UV.x by this to lengthen appearance
        _LengthScale ("UV.x Length Scale", Float) = 1.0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            fixed4 _Color;
            float _NoiseScale;
            float _NoiseStrength;
            float _Speed;
            float _AlphaFalloff;
            float _EdgeSoftness;
            float _SwayAmount;
            float _SwayFrequency;
            float _SwayFalloff;
            float _LengthScale;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;        // uv.x = along-trail (0 head -> 1 tail), uv.y = across width
                float4 color : COLOR;         // optional per-vertex color
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                fixed4 vcol : COLOR;
                float progress : TEXCOORD2;
            };

            // Sample noise texture (greyscale assumed)
            static float SampleNoise(sampler2D ntex, float2 uv)
            {
                return tex2D(ntex, uv).r;
            }

            v2f vert(appdata_t v)
            {
                v2f o;

                // Protect against missing vertex color: treat (0,0,0,0) as "no color" and use white
                fixed4 vincol = v.color;
                if (vincol.r == 0 && vincol.g == 0 && vincol.b == 0 && vincol.a == 0)
                    vincol = fixed4(1,1,1,1);

                // Compute progress from UV.x and length scale (so scaling the mesh can be compensated)
                float progress = saturate(v.uv.x * _LengthScale);
                o.progress = progress;

                // Convert vertex to world space BEFORE displacement to compute coherent noise
                float3 worldPosBefore = mul(unity_ObjectToWorld, v.vertex).xyz;

                // Build noise UV in world space; include object-scale effect automatically via worldPos
                float2 noiseUV = worldPosBefore.xz * _NoiseScale + _Time.y * _Speed;

                // Sample noise (use a low-frequency sample here)
                float n = tex2D(_NoiseTex, noiseUV * _SwayFrequency).r;

                // Map noise to -1..1
                float n01 = (n - 0.5) * 2.0;

                // Sway falls off toward the tail; head sways most
                float swayFall = pow(1.0 - progress, _SwayFalloff);

                // Width-based envelope so edges sway less (use uv.y centered)
                float widthFactor = 1.0 - abs(v.uv.y - 0.5) * 2.0; // 1 at center, 0 at edges
                widthFactor = saturate(widthFactor);

                // Final sway in world units
                float sway = n01 * _SwayAmount * swayFall * widthFactor;

                // Lateral direction in object space — change if your trail is oriented differently.
                float3 lateralObject = float3(0,1,0);

                // Apply displacement in object space (so mesh scale/rotation still apply via objectToWorld)
                float3 displaced = v.vertex.xyz + lateralObject * sway;

                // Transform displaced vertex to clip
                float4 worldPosAfter = mul(unity_ObjectToWorld, float4(displaced, 1.0));
                o.pos = UnityObjectToClipPos(float4(displaced, 1.0));

                // Pass UV and world pos after displacement for fragment coherent sampling
                o.uv = v.uv;
                o.worldPos = worldPosAfter.xyz;
                o.vcol = vincol;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Sample base texture using interpolated UV (uv.y across width)
                fixed4 baseTex = tex2D(_MainTex, i.uv) * _Color;

                // Use world-space noise for flow/flicker so effect looks consistent with world scale
                float2 noiseUV = i.worldPos.xz * _NoiseScale + _Time.y * _Speed;
                float n = tex2D(_NoiseTex, noiseUV).r;
                n = lerp(0.0, 1.0, n * _NoiseStrength);

                // Alpha falloff along trail (head = full, tail fades)
                float alphaFromProgress = pow(1.0 - saturate(i.progress), _AlphaFalloff);

                // Edge softness across width (assumes uv.y in [0,1] across width)
                float edge = smoothstep(0.0, _EdgeSoftness, i.uv.y) * (1.0 - smoothstep(1.0 - _EdgeSoftness, 1.0, i.uv.y));

                // Flicker modulation
                float noiseAlpha = lerp(0.85, 1.15, n);

                // Final alpha
                float alpha = baseTex.a * alphaFromProgress * edge * noiseAlpha * i.vcol.a;
                alpha = saturate(alpha);

                // Subtle color variation from noise
                fixed3 color = baseTex.rgb * (1.0 + (n - 0.5) * 0.35);

                return fixed4(color, alpha);
            }
            ENDCG
        }
    }

    FallBack "Unlit/Transparent"
}