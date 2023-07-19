Shader "Unlit/ActionBar"
{
    Properties
    {
        _Color("Color", Color) = (0,0,0,1)
        _MainTex ("Texture", 2D) = "white" {}
        _CornerRadius("Corner Radius", Range(0,1)) = 0.1
        _BorderWidth("Border Width", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

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
            float4 _Color;
            float _BorderWidth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                float cornerRadius = _CornerRadius / 2;

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                //Bottom left corner
                float x0 = distance(i.uv /cornerRadius, 1);
                x0 = step(x0, 1);

                // Upper left corner
                float x1 = distance((i.uv - float2(0, 1 - cornerRadius*2)) / cornerRadius, 1);
                x1 = step(x1, 1);

                // Upper right corner
                float x2 = distance((i.uv - float2(1 - cornerRadius * 2, 1 - cornerRadius * 2)) / cornerRadius, 1);
                x2 = step(x2, 1);

                // Bottom right corner
                float x3 = distance((i.uv - float2(1 - cornerRadius * 2, 0)) / cornerRadius, 1);
                x3 = step(x3, 1);

                //Vertical line
                float vertical = distance(i.uv.x + cornerRadius, cornerRadius);
                float vertical1 = distance(1-i.uv.x + cornerRadius, cornerRadius);
                vertical = step(cornerRadius, vertical);
                vertical1 = step(vertical1, cornerRadius);
                vertical = vertical-vertical1;

                //Horizontal line
                float horizontal = distance(i.uv.y + cornerRadius, cornerRadius);
                float horizontal1 = distance(1-i.uv.y + cornerRadius, cornerRadius);
                horizontal = step(cornerRadius, horizontal);
                horizontal1 = step(horizontal1, cornerRadius);
                horizontal = horizontal-horizontal1;

                float fill = vertical || horizontal || x0 || x1 || x2 || x3;

                return lerp(float4(1,1,1,0), col * _Color, fill);

            }
            ENDCG
        }
    }
}
