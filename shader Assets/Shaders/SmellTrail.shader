Shader "Custom/SmellTrail"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _NoiseTex ("Noise (R)", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _NoiseScale ("Noise Scale", Float) = 1.0
        _NoiseStrength ("Noise Strength", Range(0,1)) = 0.6
        _Speed ("Flow Speed", Float) = 1.0
        _TrailLength ("Trail Length (uv.x scale)", Float) = 1.0
        _Falloff ("Alpha Falloff", Range(0,1)) = 0.6
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        LOD 100
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            fixed4 _Color;
            float _NoiseScale;
            float _NoiseStrength;
            float _Speed;
            float _TrailLength;
            float _Falloff;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 SampleNoise(float2 uv)
            {
                return tex2D(_NoiseTex, uv);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Base color & texture
                fixed4 tex = tex2D(_MainTex, i.uv) * _Color;

                // Use uv.x as trail progress (0 = head, 1 = tail).
                // In many trail ribbons uv.x is the along-trail coordinate.
                float progress = saturate(i.uv.x / max(0.0001, _TrailLength));

                // Animated noise in world space so multiple objects share same look
                float2 noiseUV = i.worldPos.xz * _NoiseScale + _Time.y * _Speed;
                float n = tex2D(_NoiseTex, noiseUV).r;
                n = lerp(0.0, 1.0, n * _NoiseStrength);

                // Alpha falloff: stronger near head (progress = 0), fade toward tail.
                float alphaFromProgress = 1.0 - smoothstep(0.0, 1.0, progress);
                // Modulate by noise
                float alpha = alphaFromProgress * lerp(1.0, n, 0.5);
                // Apply global falloff control
                alpha *= saturate(1.0 - _Falloff * progress);

                // Multiply by texture alpha
                alpha *= tex.a;

                // Optionally tint color by noise for subtle variation
                fixed3 color = tex.rgb * lerp(1.0, 1.0 + n * 0.2, 0.5);

                return fixed4(color, alpha);
            }
            ENDCG
        }
    }

    FallBack "Unlit/Transparent"
}Shader "Custom/SmellTrail"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _NoiseTex ("Noise (R)", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _NoiseScale ("Noise Scale", Float) = 1.0
        _NoiseStrength ("Noise Strength", Range(0,1)) = 0.6
        _Speed ("Flow Speed", Float) = 1.0
        _TrailLength ("Trail Length (uv.x scale)", Float) = 1.0
        _Falloff ("Alpha Falloff", Range(0,1)) = 0.6
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        LOD 100
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            fixed4 _Color;
            float _NoiseScale;
            float _NoiseStrength;
            float _Speed;
            float _TrailLength;
            float _Falloff;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 SampleNoise(float2 uv)
            {
                return tex2D(_NoiseTex, uv);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Base color & texture
                fixed4 tex = tex2D(_MainTex, i.uv) * _Color;

                // Use uv.x as trail progress (0 = head, 1 = tail).
                // In many trail ribbons uv.x is the along-trail coordinate.
                float progress = saturate(i.uv.x / max(0.0001, _TrailLength));

                // Animated noise in world space so multiple objects share same look
                float2 noiseUV = i.worldPos.xz * _NoiseScale + _Time.y * _Speed;
                float n = tex2D(_NoiseTex, noiseUV).r;
                n = lerp(0.0, 1.0, n * _NoiseStrength);

                // Alpha falloff: stronger near head (progress = 0), fade toward tail.
                float alphaFromProgress = 1.0 - smoothstep(0.0, 1.0, progress);
                // Modulate by noise
                float alpha = alphaFromProgress * lerp(1.0, n, 0.5);
                // Apply global falloff control
                alpha *= saturate(1.0 - _Falloff * progress);

                // Multiply by texture alpha
                alpha *= tex.a;

                // Optionally tint color by noise for subtle variation
                fixed3 color = tex.rgb * lerp(1.0, 1.0 + n * 0.2, 0.5);

                return fixed4(color, alpha);
            }
            ENDCG
        }
    }

    FallBack "Unlit/Transparent"
}