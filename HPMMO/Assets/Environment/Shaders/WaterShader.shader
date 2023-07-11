Shader "Custom/FlatWaterShader" {
    Properties {
        _Color ("Water Color", Color) = (0.0, 0.5, 1.0, 1.0)
        _Glossiness ("Glossiness", Range(0, 1)) = 0.5
    }

    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        fixed4 _Color;
        float _Glossiness;

        struct Input {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
            o.Specular = _Glossiness;
        }
        ENDCG
    }

    FallBack "Diffuse"
}
