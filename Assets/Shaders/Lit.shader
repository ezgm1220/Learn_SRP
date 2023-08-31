Shader "Custom/Lit"
{
    Properties
    {
        _BaseColor("Color",Color) = (1.0,1.0,1.0,1.0)
        _BaseMap("Texture",2D) = "white" {}
      

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
