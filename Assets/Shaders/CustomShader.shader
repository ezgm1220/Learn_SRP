Shader "Custom/CustomShader"
{
    Properties
    {
        _BaseColor("Color",Color) = (1.0,1.0,1.0,1.0)
    }
    SubShader
    {
        Pass{
            HLSLPROGRAM
            #pragma shader_feature _CLIPPING
            #pragma multi_compile_instancing
            #pragma vertex ver
            #pragma fragment fra
            #include "UnlitPass.hlsl"
            ENDHLSL
        }
    }
    
}
