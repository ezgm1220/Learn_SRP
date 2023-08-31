Shader "Custom/CustomShader"
{
    Properties
    {
        _BaseColor("Color",Color) = (1.0,1.0,1.0,1.0)
        _BaseMap("Texture",2D) = "white" {}
        //设置混合模式
	    [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Src Blend", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Dst Blend", Float) = 0
        // 控制深度写入
        [Enum(Off,0,On,1)] _ZWrite("Z Write", Float) = 1
        // 透明度测试的阈值
        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
        // 控制是否开启透明度测试功能
        [Toggle(_CLIPPING)] _Clipping("Alpha Clipping", Float) = 0

    }
    SubShader
    {
        Pass{
            // 定义混合模式
            Blend[_SrcBlend][_DstBlend]
            // 是否写入深度
            ZWrite[_ZWrite]

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
