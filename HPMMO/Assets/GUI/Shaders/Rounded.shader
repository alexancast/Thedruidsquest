Shader "Unlit/Rounded"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CornerRadius("Corner Radius", float) = 0.1
        _Width("Width", float) = 1
        _Height("Height", float) = 1
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
            float _CornerRadius;
            float _Width;
            float _Height;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                float2 coord = (abs(i.uv - 0.5) + 0.5) * float2(_ScreenParams.x * _Width, _ScreenParams.y * _Height);

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                float d = length(max(abs(coord), 0));
                // d = step(d, _CornerRadius);

                return float4(d, d, d, 1);
            }
            ENDCG
        }
    }
}
