Shader "Unlit/Projection"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0,0,0,1)
        _BorderWidth ("Border Width", Range(0,1)) = 0.2
        _GradientColor ("Gradient Color", Color) = (0,0,0,1)
        _GradientSize ("Gradient Size", Range(0,1)) = 1
        _PulseSpeed ("Puls Speed", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _BorderWidth;
            float4 _Color;
            float _GradientSize;
            float _PulseSpeed;
            float _GradientColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex) * 2 - 1;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                
                //Outer Radius
                float outerRad = distance(i.uv, float2(0, 0));
                float4 outerCol = lerp(col, float4(0,0,0,0), outerRad);
                outerCol = step(0, outerCol);

                //Inner radius
                float innerRad = distance(i.uv, float2(0, 0));
                float4 innerCol = lerp(col, float4(0,0,0,0), innerRad);
                innerCol = step(_BorderWidth, innerCol);


                //Gradient
                float gradient = distance(abs(i.uv), float2(0, 0));
                float4 gradientCol = lerp(0, gradient + ((sin(_Time.y * _PulseSpeed) + 1) / 2) * _GradientSize, outerCol);

                
                col = outerCol - innerCol + gradientCol;
                col *= _Color;

                return col;
            }
            ENDCG
        }
    }
}
