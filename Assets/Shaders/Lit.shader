Shader "Custom/Lit"
{
    Properties
    {
        _BaseMap("Texture", 2D) = "white" {}
	    _BaseColor("Color", Color) = (0.5, 0.5, 0.5, 1.0)
	    //͸���Ȳ��Ե���ֵ
	    _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
	    [Toggle(_CLIPPING)] _Clipping("Alpha Clipping", Float) = 0
        //͸��ͨ��Ԥ��
	    [Toggle(_PREMULTIPLY_ALPHA)] _PremulAlpha("Premultiply Alpha", Float) = 0
        //�����Ⱥ͹⻬��
	    _Metallic("Metallic", Range(0, 1)) = 0
        _Smoothness("Smoothness", Range(0, 1)) = 0.5
	    //���û��ģʽ
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Src Blend", Float) = 1
	    [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Dst Blend", Float) = 0
	    //Ĭ��д����Ȼ�����
	    [Enum(Off, 0, On, 1)] _ZWrite("Z Write", Float) = 1

    }
    SubShader
    {
        Pass
        {
		   Tags {
				"LightMode" = "CustomLit"
			}

		   //������ģʽ
		   Blend[_SrcBlend][_DstBlend]
		   //�Ƿ�д�����
		   ZWrite[_ZWrite]
           HLSLPROGRAM
		   #pragma target 3.5
		   #pragma shader_feature _CLIPPING
		   //�Ƿ�͸��ͨ��Ԥ��
		   #pragma shader_feature _PREMULTIPLY_ALPHA
           #pragma multi_compile_instancing
           #pragma vertex LitVer
           #pragma fragment LitFra
		   //�������hlsl����
           #include"LitPass.hlsl"
           ENDHLSL
        }

		Pass
        {
            Tags 
            {
                "LightMode" = "ShadowCaster"
            }
            ColorMask 0

                HLSLPROGRAM
                #pragma target 3.5
                #pragma shader_feature _CLIPPING
                #pragma multi_compile_instancing
                #pragma vertex ShadowVer
                #pragma fragment ShadowFra
                #include "ShadowCasterPass.hlsl"
                ENDHLSL
        }
    }
    CustomEditor "CustomShaderGUI"
}
