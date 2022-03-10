Shader "Unlit/Citizen"
{
    Properties {
          [PreRendererData]_MainTex("Sprite Texture", 2D) = "white"{}
        _difference("black/white difference", Range(0,5)) = 1
        _changePoint("black/white change point", Range(0,1)) = .5
        _outerColor("omni outer color", Color) = (1,1,1,1)
        _innerColor("omni inner color", Color) = (0,0,0,0)
    }
     SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            float _difference;
            float _changePoint;
            float4 _outerColor;
            float4 _innerColor;

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (Interpolators i) : SV_Target
            {
                float4 color = 0;
                float2 uv = i.uv * 2 - 1;
                
                float opacity = length(uv);
                float4 blackCircle = length(uv)-_changePoint;
                blackCircle = pow(blackCircle,_difference);
                float radius = 0.66;

                opacity = length(uv) - radius;
                opacity = step(0,opacity);
                opacity = 1-opacity;
                                
                blackCircle.a = opacity;
                blackCircle.rgb *= _outerColor;
                color = blackCircle;
                return color;
            }
            ENDCG
        }
    }
}
