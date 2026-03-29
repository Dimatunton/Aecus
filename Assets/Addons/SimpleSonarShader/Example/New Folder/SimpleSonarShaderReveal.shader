Shader "MadeByProfessorOakie/SimpleSonarShaderReveal"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Color("Color Tint", Color) = (1,1,1,1)
        _HiddenColor("Hidden Color", Color) = (0,0,0,1)
        _RingColor("Ring Color", Color) = (1,1,1,1)
        _RingWidth("Ring Width", float) = 0.1
        _RingSpeed("Ring Speed", float) = 1
        _RingIntensity("Ring Intensity", float) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        fixed4 _Color;
        fixed4 _HiddenColor;
        fixed4 _RingColor;
        float _RingWidth;
        float _RingSpeed;
        float _RingIntensity;

        // Sonar ring data (single ring for simplicity, can be extended)
        float4 _SonarOrigin; // xyz = world pos, w = start time

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // Calculate distance from sonar origin to this pixel
            float d = distance(_SonarOrigin.xyz, IN.worldPos);

            // Time since ring started
            float t = (_Time.y - _SonarOrigin.w) * _RingSpeed;

            // Ring mask: 1 inside the ring, 0 outside, soft edge
            float ringMask = smoothstep(_RingWidth, 0, abs(d - t));

            // Reveal: 1 where ring is, 0 elsewhere
            float reveal = ringMask;

            // Sample the object's texture
            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex) * _Color;

            // Mix between hidden color and real texture based on reveal
            o.Albedo = lerp(_HiddenColor.rgb, tex.rgb, reveal);

            // Optional: add a colored ring edge for visual feedback
            float ringEdge = smoothstep(0.02, 0, abs(d - t));
            o.Emission = _RingColor.rgb * ringEdge * _RingIntensity;

            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}