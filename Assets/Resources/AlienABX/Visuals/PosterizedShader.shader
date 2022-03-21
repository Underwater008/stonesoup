Shader "Custom/Posterized"
{
    Properties 
    {
        _MainTex ("render texture", 2D) = "white"{}
        _steps ("steps", Range(1,16)) = 16
        _darkColor ("replace color", Color) = (0,0,0,0)
        _sep("color separation",Int) = 3
        _abIntensity("chromatic abberation intensity", Range(0,.1)) = 0.01
    }
    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            #define MAX_OFFSET 0.15

            sampler2D _MainTex; float4 _MainTex_TexelSize;
            float _steps;
            float3 _c1;
            float3 _darkColor;
            float3 _c3;
            int _sep;
            float _abIntensity;


            float white_noise (float2 value) {
                float uv = value;
                uv = floor(uv*600); //find the number for like it to not flicker
                float wn = 0;
                float samp = dot(uv,float2(128.239,-78.382));
                
                wn = frac(sin(samp)*90321);
                return wn;
            }

            float rand (float2 uv) {
                return frac(sin(dot(uv.xy, float2(12.9898, 78.233))) * 43758.5453123);
            }

            float value_noise (float2 uv) {
                float2 ipos = floor(uv);
                float2 fpos = frac(uv); 
                
                float o  = rand(ipos);
                float x  = rand(ipos + float2(1, 0));
                float y  = rand(ipos + float2(0, 1));
                float xy = rand(ipos + float2(1, 1));

                float2 smooth = smoothstep(0, 1, fpos);
                return lerp( lerp(o,  x, smooth.x), 
                             lerp(y, xy, smooth.x), smooth.y);
            }

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
                float3 color = 0;
                float2 uv = i.uv;
                float wn = value_noise(uv*400);
                float3 grayscaleReference = float3(0.299,0.587,0.114);
                color = tex2D(_MainTex,uv);
                float grayscale = dot(color,grayscaleReference);
                float steps = _steps;
                 grayscale = floor((grayscale)*_steps+wn)/(_steps);


                float modifier = length(uv*2-1);
                float offset = MAX_OFFSET*_abIntensity * modifier;
                float r = tex2D(_MainTex,uv - offset*wn).r;
                float g = tex2D(_MainTex,uv).g;
                float b = tex2D(_MainTex,uv + offset/wn).b;

                float3 c1 = float3(r,g,b);
                
                float3 recolor = lerp(_darkColor,c1,grayscale);
                recolor = pow(recolor,_sep);

                

                return float4(recolor, 1.0);
            }
            ENDCG
        }
    }
}
