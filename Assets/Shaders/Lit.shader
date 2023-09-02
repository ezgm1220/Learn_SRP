Shader "Custom/Lit"
{
    Properties
    {
        _BaseColor("Color",Color) = (1.0,1.0,1.0,1.0)
        _BaseMap("Texture",2D) = "white" {}
        //金属度和光滑度
	    _Metallic("Metallic", Range(0, 1)) = 0
	    _Smoothness("Smoothness", Range(0, 1)) = 0.5

    }
    SubShader
    {
        Pass{
            
            Tags {
				"LightMode" = "CustomLit"
			}

            HLSLPROGRAM
            #pragma vertex LitVer
            #pragma fragment LitFra
            #include "LitPass.hlsl"
            ENDHLSL
        }
    }
    
}
