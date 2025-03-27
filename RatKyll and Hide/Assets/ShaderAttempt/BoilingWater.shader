Shader "Custom/BoilingWater"
{
    Properties
    {
        _Color ("Water Color", Color) = (0, 0.5, 1, 1)
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _Speed ("Boil Speed", Float) = 1.0
        _Distortion ("Distortion Strength", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _NoiseTex;
            sampler2D _NormalMap;
            float4 _Color;
            float _Speed;
            float _Distortion;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float timeOffset = _Time.y * _Speed;

                // Boiling effect using noise texture animation
                float2 noiseUV = i.uv + float2(sin(timeOffset), cos(timeOffset)) * 0.02;
                float noise = tex2D(_NoiseTex, noiseUV).r;

                // Normal map movement for small bubbling
                float2 normalUV = i.uv + float2(sin(timeOffset * 1.2), cos(timeOffset * 1.5)) * 0.05;
                float3 normalData = UnpackNormal(tex2D(_NormalMap, normalUV));

                // Distortion effect
                float2 distortionOffset = normalData.rg * _Distortion * noise;
                float2 finalUV = i.uv + distortionOffset;

                // Color blending
                fixed4 finalColor = _Color;
                finalColor.a = 0.8 + noise * 0.2; // Alpha variation for transparency

                return finalColor;
            }
            ENDCG
        }
    }
}
