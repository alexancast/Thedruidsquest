Shader "Custom/Spellbar"
{
    Properties
    {
        _FillAmount ("Fill Amount", Range(0, 1)) = 1
        _MainTex ("Texture", 2D) = "white" {}
        _TextureOffset("Texture Offset", float) = 0
        _TextureScale("Texture Scale", float) = 0
        _TextureInfluence("Texture Influence", Range(0,1)) = 1
        _FillColor("Fill Color", Color) = (0,0,0,0)
        _EmptyColor("Empty Color", Color) = (0,0,0,0)
        _BorderColor("Border Color", Color) = (0,0,0,0)
        _Width("Width", Range(0,1)) = 1
        _Height("Height", Range(0,1)) = 1
        _ScrollSpeed("Scroll Speed", float) = 1
        _BorderWidth("BorderWidth", Range(0,1)) = 0.1

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
            float _Width;
            float _Height;
            float _ScrollSpeed;
            float _TextureOffset;
            float _TextureScale;
            float _TextureInfluence;
            float _BorderWidth;
            float4 _BorderColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * 2 - 1;
                return o;
            }

            float invLerp(float a, float b, float value){

                return (value - a) / (b - a);
            }

            float remap(float iMin, float iMax, float oMin, float oMax, float value){

                float t = invLerp(iMin, iMax, value);
                return lerp(oMin, oMax, t);
            }

            fixed4 frag(v2f i) : SV_Target
            {
            
                //Setting the shape of the bar
                float fill = distance(float2(_Width-_Height,0), abs(i.uv));
                fill = step(fill, _Height) || (abs(i.uv.x) < _Width - _Height && abs(i.uv.y) < _Height);
                fixed4 col = lerp(fixed4(0,0,0,0), fixed4(1,1,1,1), fill);


                //Setting the shape of the border
                float borderFill = distance(float2(_Width - _Height, 0), abs(i.uv));
                borderFill = step(borderFill, _Height - _BorderWidth) || (abs(i.uv.x) < _Width - _Height && abs(i.uv.y) < _Height -_BorderWidth);
                // float4 borderCol = lerp(fixed4(0,0,0,1), fixed4(1,1,1,1), borderFill);


                //Setting the color of the bar
                float fillAmount = lerp(-1, 1, _FillAmount);
                fill = step(fillAmount, i.uv.x);
                float2 offset = float2(i.uv.x - _Time.y * _ScrollSpeed, i.uv.y + _TextureOffset) * _TextureScale;

                //Applying texture to bar
                float4 tex = tex2D(_MainTex, offset);
                tex.a = lerp(0, 1, tex); 
                col *= lerp(_FillColor + _TextureInfluence * tex, _EmptyColor, fill);

                col = lerp(col, _BorderColor, 1-borderFill);
                
                //Repeated code, could probably be done better
                fill = distance(float2(_Width-_Height,0), abs(i.uv));
                fill = step(fill, _Height) || (abs(i.uv.x) < _Width - _Height && abs(i.uv.y) < _Height);

                col *= fill;

                return col;

            }
            ENDCG
        }
    }
}
