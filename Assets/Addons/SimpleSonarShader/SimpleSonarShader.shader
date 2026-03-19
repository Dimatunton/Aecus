// SimpleSonarShader scripts and shaders were written by Drew Okenfuss.
// Modified to support Emission and an Unlit toggle
// Updated: Outline uses object-space normals (fixes floating plane look), added _OutlineColor and _OutlineWidth for control

Shader "MadeByProfessorOakie/SimpleSonarShaderEmission"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}

        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0

        _RingColor("Ring Color", Color) = (1,1,1,1)
        _RingColorIntensity("Ring Color Intensity", float) = 2

        _EmissionColor("Emission Color", Color) = (1,1,1,1)
        _EmissionStrength("Emission Strength", Float) = 1

        _RingSpeed("Ring Speed", float) = 1
        _RingWidth("Ring Width", float) = 0.1
        _RingIntensityScale("Ring Range", float) = 1

        _RingTex("Ring Texture", 2D) = "white" {}

        // New properties for unlit mode
        _UnlitToggle("Unlit Mode (0 = off, 1 = on)", Range(0,1)) = 0
        _UnlitIntensity("Unlit Intensity", Float) = 1

        // Outline properties
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth("Outline Width", Range(0,0.2)) = 0.02
    }

    SubShader
    {
        Tags{ "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM

        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _RingTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        // max sonar rings
        half4 _hitPts[20];
        half _StartTime;
        half _Intensity[20];

        half _Glossiness;
        half _Metallic;

        fixed4 _Color;

        fixed4 _RingColor;
        half _RingColorIntensity;

        fixed4 _EmissionColor;
        half _EmissionStrength;

        half _RingSpeed;
        half _RingWidth;
        half _RingIntensityScale;

        // new unlit controls
        half _UnlitToggle;
        half _UnlitIntensity;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

            o.Albedo = c.rgb;

            half DiffFromRingCol =
                abs(o.Albedo.r - _RingColor.r) +
                abs(o.Albedo.g - _RingColor.g) +
                abs(o.Albedo.b - _RingColor.b);

            // emission accumulator
            half3 emissionAccum = 0;

            for (int i = 0; i < 20; i++)
            {
                half d = distance(_hitPts[i], IN.worldPos);

                half intensity = _Intensity[i] * _RingIntensityScale;

                half val = (1 - (d / intensity));

                if (d < (_Time.y - _hitPts[i].w) * _RingSpeed &&
                    d > (_Time.y - _hitPts[i].w) * _RingSpeed - _RingWidth &&
                    val > 0)
                {
                    half posInRing =
                        (d - ((_Time.y - _hitPts[i].w) * _RingSpeed - _RingWidth))
                        / _RingWidth;

                    float angle =
                        acos(dot(normalize(IN.worldPos - _hitPts[i]),
                        float3(1,0,0)));

                    val *= tex2D(_RingTex, half2(1 - posInRing, angle));

                    half3 tmp = _RingColor * val + c * (1 - val);

                    half tempDiffFromRingCol =
                        abs(tmp.r - _RingColor.r) +
                        abs(tmp.g - _RingColor.g) +
                        abs(tmp.b - _RingColor.b);

                    if (tempDiffFromRingCol < DiffFromRingCol)
                    {
                        DiffFromRingCol = tempDiffFromRingCol;

                        o.Albedo = tmp * _RingColorIntensity;

                        // accumulate emission from ring
                        emissionAccum += _RingColor.rgb * val;
                    }
                }
            }

            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;

            // baseline emission from rings
            o.Emission = emissionAccum * _EmissionColor.rgb * _EmissionStrength;

            // Unlit mode: add the object's final color as emission so it is visible in dark.
            // When _UnlitToggle == 1 the object's albedo is suppressed so lighting doesn't dim it.
            half unlit = saturate(_UnlitToggle);
            half3 finalColor = o.Albedo;
            o.Emission += finalColor * _EmissionColor.rgb * _EmissionStrength * _UnlitIntensity * unlit;
            o.Albedo = lerp(finalColor, finalColor * 0, unlit);
        }

        ENDCG

        // Outline pass (inverted hull) - must be inside the SubShader
        // Uses object-space normals for consistent extrusion (fixes floating-plane artifact).
        Pass
        {
            Name "OUTLINE"
            Tags { "LightMode" = "Always" }
            Cull Front
            ZWrite Off
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"

            // properties used by outline pass
            float _UnlitToggle;
            float _OutlineWidth;
            fixed4 _OutlineColor;
            float _UnlitIntensity; // still available if you want intensity-driven thickness

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                // use object-space normal (v.normal) and object-space vertex (v.vertex)
                // normalize the normal to make extrusion stable
                float3 n = normalize(v.normal);
                // extrusion amount in object-space units; users can tweak _OutlineWidth
                float extrude = _OutlineWidth * saturate(_UnlitToggle);
                // extrude along normal (outwards). Using object-space keeps the outline attached to geometry.
                float3 posOffset = v.vertex.xyz + n * extrude;
                o.pos = UnityObjectToClipPos(float4(posOffset, 1.0));
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // alpha driven by toggle so outline hides when unlit mode is off
                float a = saturate(_UnlitToggle);
                return fixed4(_OutlineColor.rgb, _OutlineColor.a * a);
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}