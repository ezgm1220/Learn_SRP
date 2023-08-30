Shader "Custom/CustomShader"
{
    Properties
    {
        
    }
    SubShader
    {
        Pass{
            HLSLPROGRAM
            #pragma vertex ver
            #pragma fragment fra
            #include "UnlitPass.hlsl"
            ENDHLSL
        }
    }
    
}
