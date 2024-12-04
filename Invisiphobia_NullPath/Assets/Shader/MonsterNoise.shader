Shader "Hidden/Noise"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoisePower("Noise Power", Range(0, 0.1)) = 0.01
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float2 nrand(float x, float y)
            {
                return frac(sin(dot(float2(x, y), float2(12.9898, 78.233))) * 43758.5453);
            }

            sampler2D _MainTex;
            float _NoisePower;

            fixed4 frag(v2f i) : SV_Target
            {
                float2 add = _NoisePower * nrand(i.vertex.x, _Time.x);
                fixed4 col = tex2D(_MainTex, i.uv + add);
                

                return col;
            }
            ENDCG
        }
    }
}
