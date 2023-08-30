#ifndef CUSTOM_UNLIT_PASS_INCLUDED
#define CUSTOM_UNLIT_PASS_INCLUDED

#include "ShaderLibrary/Common.hlsl"

//���㺯������ṹ��
struct Attributes {
	float3 positionOS : POSITION;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

//ƬԪ��������ṹ��
struct Varyings {
	float4 positionCS : SV_POSITION;
	float2 baseUV : VAR_BASE_UV;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

Varyings ver(Attributes input) 
{
	Varyings output;
	UNITY_SETUP_INSTANCE_ID(input);// ��ȡ��Ⱦ���������

	//ʹUnlitPassVertex���λ�ú�����,����������
	UNITY_TRANSFER_INSTANCE_ID(input, output);

	float3 positionWS = TransformObjectToWorld(input.positionOS);
	output.positionCS = TransformWorldToHClip(positionWS);

	////�������ź�ƫ�ƺ��UV����
	//float4 baseST = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _BaseMap_ST);
	//output.baseUV = input.baseUV * baseST.xy + baseST.zw;

	return output;
}

//CBUFFER_START(UnityPerMaterial)
//	float4 _BaseColor;
//CBUFFER_END

UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)
UNITY_DEFINE_INSTANCED_PROP(float4, _BaseColor)
UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)

float4 fra(Varyings input) : SV_TARGET
{
	UNITY_SETUP_INSTANCE_ID(input);// ��ȡ��Ⱦ���������

	return UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _BaseColor);
}

#endif