Shader "Unlit/Omnisphere"
{
    Properties {
        [PreRendererData]_MainTex("Sprite Texture", 2D) = "white"{}
        _radius("omnisphere radius", Range(.5,1)) = .7
        _difference("membrane thickness", Range(.1,.5)) = .2
        _membraneColor("membrane color", Color) = (1,1,1,1)
        _innerColor("omni inner color", Color) = (1,1,1,1)

        _bandThinkness("moving line thickness", Range(.01,.1)) = .1
        _bandSpeed("moving line speed", Range(.1,1)) = .5
        _bandCount("amount of moving lines", Int) = 3
        _bandColor("band color", Color) = (1,1,1,1)
        _bandsOn("bandsOnVal", Range(0,1)) = 1
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
            float4 _membraneColor;
            float4 _innerColor;
            float _radius;
            float _bandThinkness;
            float _bandSpeed;
            float _bandCount;
            float4 _bandColor;
            float _bandsOn;

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

            float circle(float radius, float2 uv){
                return step(1-radius, 1-length(uv));
            }
            float4 frag (Interpolators i) : SV_Target
            {
                float4 color = 0;
                float2 uv = i.uv * 2 - 1;
                float radius = _radius;
                

                //omnisphere base
                float4 outerCircle = circle(radius,uv);
                float4 blackInside = circle(radius-_difference,uv);
                outerCircle -= blackInside;
                outerCircle*=_membraneColor;
                blackInside*=_innerColor;
                color = outerCircle+blackInside;


                //moving lines
                radius = sin(radius);
                for(int i = 0; i <_bandCount; i++){
                    float bandRadius = radius-frac(_Time.z*_bandSpeed*i);
                    float4 band = circle(bandRadius,uv);
                    band = band - circle(bandRadius-_bandThinkness,uv);
                    color += (band*_bandColor)*_bandsOn;
                }
                
                return color;
            }
            ENDCG
        }
    }
}
