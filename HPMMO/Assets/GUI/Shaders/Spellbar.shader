Shader "Custom/Spellbar"
{
    Properties
    {
        _FillAmount ("Fill Amount", Range(0, 1)) = 1
        _MainTex ("Texture", 2D) = "white" {}
        _FillColor("Fill Color", Color) = (0,0,0,0)
        _EmptyColor("Empty Color", Color) = (0,0,0,0)
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" }
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
            float _FillAmount;
            float4 _FillColor;
            float4 _EmptyColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Calculate the fill amount
                float fill = step(i.uv.x, _FillAmount);

                // Interpolate between the fill and empty color based on fill amount
                fixed4 col = lerp(_EmptyColor, _FillColor, fill);

                return col;
            }
            ENDCG
        }
    }
}
