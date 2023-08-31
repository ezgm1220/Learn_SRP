Shader "Custom/CustomShader"
{
    Properties
    {
        _BaseColor("Color",Color) = (1.0,1.0,1.0,1.0)
        _BaseMap("Texture",2D) = "white" {}
        //���û��ģʽ
	    [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Src Blend", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Dst Blend", Float) = 0
        // �������д��
        [Enum(Off,0,On,1)] _ZWrite("Z Write", Float) = 1
        // ͸���Ȳ��Ե���ֵ
        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
        // �����Ƿ���͸���Ȳ��Թ���
        [Toggle(_CLIPPING)] _Clipping("Alpha Clipping", Float) = 0

    }
    SubShader
    {
        Pass{
            // ������ģʽ
            Blend[_SrcBlend][_DstBlend]
            // �Ƿ�д�����
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
