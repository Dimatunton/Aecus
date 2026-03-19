Shader "Custom/SmellTrail"
{
    Properties
    {
        _MainTex ("Main Texture (RGBA)", 2D) = "white" {}
        _NoiseTex ("Noise (R)", 2D) = "white" {}
        _Color ("Tint", Color) = (1,0.6,0.2,1)

        // Main texture scaling controls
        _MainTexScale ("Main Texture Scale (X,Y)", Vector) = (1,1,0,0)
        _MainTexUseWorld ("MainTex Use World Coords (0=UV,1=World)", Float) = 0
        _MainTexWorldScale ("MainTex World Scale (tiles per world unit)", Float) = 1.0

        _NoiseScale ("Noise Scale", Float) = 1.0
        _NoiseStrength ("Noise Strength", Range(0,2)) = 0.45
        _Speed ("Flow Speed", Float) = 1.0
        _AlphaFalloff ("Alpha Falloff (tail)", Range(0,4)) = 1.5
        _HeadFalloff ("Alpha Falloff (head)", Range(0,4)) = 1.5
        _EdgeSoftness ("Edge Softness", Range(0,1)) = 0.2

        // Sway (FBM) in world units
        _SwayAmount ("Sway Amount (world)", Float) = 0.5
        _SwayFrequency ("Sway Frequency", Float) = 1.0
        _SwayFalloff ("Sway Falloff", Float) = 1.8

        // Traveling wave along trail (world units)
        _WaveAmplitude ("Wave Amplitude (world)", Float) = 0.15
        _WaveFrequencyAlong ("Wave Frequency (cycles along trail)", Float) = 2.0
        _WaveSpeed ("Wave Speed (cycles/sec)", Float) = 1.0

        // Length mapping (uv.x multiplier)
        _LengthScale ("UV.x Length Scale", Float) = 1.0

        // Optional: define a real world length for the trail so progress doesn't depend on UVs or scale.
        _WorldLength ("World Length (0 = use UV.x)", Float) = 0.0

        // Temporal smoothing (0..1). Higher = smoother but more lag.
        _TemporalBlend ("Temporal Blend", Range(0,1)) = 0.6
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" "DisableBatching"="True" }
        LOD 200
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        // disable fixed-function lighting for this pass so it is unaffected by scene lights
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;

            float4 _Color;
            float4 _MainTexScale;
            float _MainTexUseWorld;
            float _MainTexWorldScale;
            float _NoiseScale;
            float _NoiseStrength;
            float _Speed;
            float _AlphaFalloff;
            float _HeadFalloff;
            float _EdgeSoftness;
            float _SwayAmount;
            float _SwayFrequency;
            float _SwayFalloff;
            float _WaveAmplitude;
            float _WaveFrequencyAlong;
            float _WaveSpeed;
            float _LengthScale;
            float _WorldLength;
            float _TemporalBlend;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0; // uv.x = along-trail, uv.y = across width
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float4 vcol : COLOR;
                float progress : TEXCOORD2;
            };

            // value-noise helpers (quintic interpolation)
            static float hash21(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
            }

            static float valueNoise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                // quintic interpolation
                float2 u = f * f * f * (f * (f * 6.0 - 15.0) + 10.0);

                float a = hash21(i + float2(0.0, 0.0));
                float b = hash21(i + float2(1.0, 0.0));
                float c = hash21(i + float2(0.0, 1.0));
                float d = hash21(i + float2(1.0, 1.0));

                float x1 = lerp(a, b, u.x);
                float x2 = lerp(c, d, u.x);
                return lerp(x1, x2, u.y);
            }

            // FBM: reduced HF energy and temporal-friendly
            static float fbm_noise(float2 p)
            {
                float sum = 0.0;
                float amp = 0.6; // base amplitude
                float freq = 1.0;
                // 5 octaves with faster amp falloff to reduce jitter
                for (int i = 0; i < 5; ++i)
                {
                    float angle = 0.314159 * i;
                    float2 rp = float2(p.x * cos(angle) - p.y * sin(angle),
                                       p.x * sin(angle) + p.y * cos(angle));
                    sum += amp * valueNoise(rp * freq);
                    freq *= 2.0;
                    amp *= 0.45; // stronger amplitude falloff => smoother
                }
                return saturate(sum);
            }

            // multi-harmonic traveling wave (smooth)
            static float travelingWave(float progress, float time, float freqAlong, float baseSpeed, int harmonics)
            {
                const float PI2 = 6.28318530718;
                float total = 0.0;
                float amp = 1.0;
                float norm = 0.0;
                // 2-3 harmonics is enough; keep gentle
                for (int h = 0; h < harmonics; ++h)
                {
                    float f = freqAlong * (1.0 + 0.4 * h);
                    total += amp * sin(progress * (f * PI2) + time * baseSpeed * PI2 * (1.0 + 0.05 * h));
                    norm += amp;
                    amp *= 0.45;
                }
                return total / max(norm, 1e-6);
            }

            v2f vert(appdata_t v)
            {
                v2f o;

                float4 vincol = v.color;
                if (vincol.r == 0 && vincol.g == 0 && vincol.b == 0 && vincol.a == 0)
                    vincol = float4(1,1,1,1);

                // world position BEFORE displacement
                float3 worldPosBefore = mul(unity_ObjectToWorld, v.vertex).xyz;

                // compute progress: either from UV.x or from world-space along local X if _WorldLength > 0
                float progress;
                if (_WorldLength > 0.0001)
                {
                    float3 localX = normalize(unity_ObjectToWorld[0].xyz);
                    float3 objWorldPos = unity_ObjectToWorld[3].xyz;
                    float worldDist = dot(worldPosBefore - objWorldPos, localX);
                    progress = saturate(worldDist / max(_WorldLength, 1e-6));
                }
                else
                {
                    progress = saturate(v.uv.x * _LengthScale);
                }
                o.progress = progress;

                // FBM-based sway sampling in world space with cheap temporal low-pass:
                // sample three time offsets and blend (reduces frame-to-frame jitter without frame-history)
                float t = _Time.y * _Speed;
                float2 nPos = worldPosBefore.xz * (_NoiseScale * _SwayFrequency);

                // time offsets chosen to decorrelate high-frequency fluctuations
                float n0 = fbm_noise(nPos + float2(t, t));
                float n1 = fbm_noise(nPos + float2(t + 0.37, t + 0.37));
                float n2 = fbm_noise(nPos + float2(t - 0.23, t - 0.23));

                // weights tuned: give more weight to current, mix in neighbors, then apply user temporal blend
                float raw = n0 * 0.6 + n1 * 0.25 + n2 * 0.15;
                // final temporal mix with user control (0 = raw, 1 = stronger smoothing toward multi-sample)
                float n = lerp(n0, raw, clamp(_TemporalBlend, 0.0, 1.0));
                float n01 = (n - 0.5) * 2.0;

                // width envelope and falloff
                float widthFactor = saturate(1.0 - abs(v.uv.y - 0.5) * 2.0);
                float swayFall = pow(1.0 - progress, _SwayFalloff);

                // FBM sway in world units
                float swayWorldFBM = n01 * _SwayAmount * swayFall * widthFactor;

                // traveling wave (gentle)
                float waveNormalized = travelingWave(progress, _Time.y * _WaveSpeed, _WaveFrequencyAlong, _WaveSpeed, 2);
                float waveWorld = waveNormalized * _WaveAmplitude * swayFall * widthFactor;

                // combine sway and wave (both in world units)
                float swayWorld = swayWorldFBM + waveWorld;

                // convert world sway to object-space offset by dividing local Y scale
                float3 osY = unity_ObjectToWorld[1].xyz;
                float scaleY = max(length(osY), 1e-6);
                float swayObjectSpace = swayWorld / scaleY;

                // lateral direction in object space (local Y)
                float3 lateralObject = float3(0,1,0);
                float3 displaced = v.vertex.xyz + lateralObject * swayObjectSpace;

                // output
                float4 worldPosAfter = mul(unity_ObjectToWorld, float4(displaced, 1.0));
                o.pos = UnityObjectToClipPos(float4(displaced, 1.0));
                o.uv = v.uv;
                o.worldPos = worldPosAfter.xyz;
                o.vcol = vincol;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // choose main texture UVs: either world-space or mesh UV scaled by _MainTexScale
                float2 baseUV;
                if (_MainTexUseWorld > 0.5)
                {
                    baseUV = i.worldPos.xz * _MainTexWorldScale;
                }
                else
                {
                    baseUV = i.uv * _MainTexScale.xy;
                }

                // texture and tint
                fixed4 baseTex = tex2D(_MainTex, baseUV);
                fixed3 baseColor = baseTex.rgb * _Color.rgb;
                float baseAlpha = baseTex.a * _Color.a;

                // fragment noise for subtle color variation (smaller strength reduces flicker)
                float2 noiseUV = i.worldPos.xz * _NoiseScale + float2(_Time.y * _Speed, _Time.y * _Speed);
                float n = tex2D(_NoiseTex, noiseUV * _SwayFrequency).r;
                // soften the influence of noise using pow to reduce abrupt jumps
                float nScaled = pow(saturate(n * saturate(_NoiseStrength)), 1.25);

                // alpha falloff at both head and tail:
                float head = pow(saturate(i.progress), max(0.0001, _HeadFalloff));
                float tail = pow(1.0 - saturate(i.progress), max(0.0001, _AlphaFalloff));
                float alphaFromProgress = head * tail;

                float edge = smoothstep(0.0, _EdgeSoftness, i.uv.y) * (1.0 - smoothstep(1.0 - _EdgeSoftness, 1.0, i.uv.y));
                float noiseAlpha = lerp(0.95, 1.05, nScaled);

                float alpha = baseAlpha * alphaFromProgress * edge * noiseAlpha * i.vcol.a;
                alpha = saturate(alpha);

                fixed3 color = baseColor * (1.0 + (nScaled - 0.5) * 0.18);

                return fixed4(color, alpha);
            }
            ENDCG
        }
    }

    FallBack "Unlit/Transparent"
}