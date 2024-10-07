Shader "Custom/FogOfWarShader"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _FogTex ("Fog Texture", 2D) = "white" {}
        _FogColor ("Fog Color", Color) = (0,0,0,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

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
            sampler2D _FogTex;
            float4 _FogColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 baseColor = tex2D(_MainTex, i.uv);
                fixed4 fogColor = tex2D(_FogTex, i.uv);
                return lerp(_FogColor, baseColor, fogColor.a);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
