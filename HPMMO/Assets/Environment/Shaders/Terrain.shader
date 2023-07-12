Shader "Custom/Terrain"
{
    Properties
    {
        _PlayerPos ("Player Position", Vector) = (0,0,0)
        _GrassColor ("Grass Color", Color) = (1,1,1,1)
        _MountainColor ("Mountain Color", Color) = (1,1,1,1)
        _BlendSharpness ("Blend Sharpness", Range(0, 1)) = 0.1
        _TerrainHeight("Terrain Height", float) = 10
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Radius ("Draught Radius", Range(0, 10)) = 0.5
        _SmoothingRadius ("Smoothing radius", Range(0, 5)) = 0.5


    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays
        #pragma exclude_renderers d3d11 gles
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldNormal;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _GrassColor;
        fixed4 _MountainColor;
        float _BlendSharpness;
        float _TerrainHeight;
        float3 _PlayerPos;
        float _Radius;
        float _SmoothingRadius;


        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)


        float invLerp(float a, float b, float value){

            return (value - a) / (b - a);
        }

        float remap(float iMin, float iMax, float oMin, float oMax, float value){

            float t = invLerp(iMin, iMax, value);
            return lerp(oMin, oMax, t);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            /// Beräkna normalens vinkel i förhållande till uppåtriktningen (0, 1, 0)
            float angle = dot(IN.worldNormal, float3(0, 1, 0));

            // Ange tröskelvinkeln (i grader) där "_MountainColor" ska användas
            float thresholdAngle = 0.5;

            // Jämför normalvinkeln med tröskelvärdet och välj färgen baserat på resultatet
            float blendFactor = smoothstep(0, _BlendSharpness, abs(angle - cos(radians(thresholdAngle))));
            float3 angleCol = lerp(_GrassColor.rgb, _MountainColor.rgb, blendFactor);
            

            float3 heightCol = lerp(_MountainColor.rgb, _GrassColor.rgb, IN.worldPos.y/_TerrainHeight);


            float3 col = lerp(heightCol, _MountainColor, blendFactor);


            float dist = distance(IN.uv_MainTex, _PlayerPos.xz);
            float alpha = 1 - smoothstep(_Radius - _SmoothingRadius, _Radius + _SmoothingRadius, dist);

            dist = clamp(dist, 0, _Radius);
            col = lerp(col, _MountainColor, alpha);
            
            o.Albedo = col;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
