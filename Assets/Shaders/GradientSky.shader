Shader "Custom/GradientSky"
{
    Properties
    {
        _TopColor ("Top Color", Color) = (1,1,1,1)
        _BottomColor ("Bottom Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard noshadow
        #pragma target 3.0

        fixed4 _TopColor;
        fixed4 _BottomColor;

        struct Input
        {
            float4 screenPos;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
            fixed4 c = lerp(_BottomColor, _TopColor, screenUV.y);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "VertexLit"
}