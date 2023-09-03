#ifndef CUSTOM_UNLIT_PASS_INCLUDED
#define CUSTOM_UNLIT_PASS_INCLUDED

#include "../ShaderLibrary/Common.hlsl"
#include "../ShaderLibrary/Surface.hlsl"
#include "../ShaderLibrary/Shadows.hlsl"
#include "../ShaderLibrary/Light.hlsl"
#include "../ShaderLibrary/BRDF.hlsl"
#include "../ShaderLibrary/Lighting.hlsl"

TEXTURE2D(_BaseMap);
SAMPLER(sampler_BaseMap);

UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)// �����������ĳ�������������ÿ��ʵ��������

UNITY_DEFINE_INSTANCED_PROP(float4, _BaseMap_ST)// �ṩ��������ź�ƽ��
UNITY_DEFINE_INSTANCED_PROP(float4, _BaseColor)
UNITY_DEFINE_INSTANCED_PROP(float, _Cutoff)
UNITY_DEFINE_INSTANCED_PROP(float, _Metallic)
UNITY_DEFINE_INSTANCED_PROP(float, _Smoothness)

UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)

// ���㺯������ṹ��
struct Attributes {
	float3 positionOS : POSITION;
	float2 baseUV : TEXCOORD0;
	// ���淨��
	float3 normalOS : NORMAL;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

// ƬԪ��������ṹ��
struct Varyings {
	float4 positionCS : SV_POSITION;
	float3 positionWS : VAR_POSITION;
	float2 baseUV : VAR_BASE_UV;
	// ���編��
	float3 normalWS : VAR_NORMAL;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

Varyings LitVer(Attributes input) 
{
	Varyings output;
	UNITY_SETUP_INSTANCE_ID(input);// ��ȡ��Ⱦ���������

	//ʹUnlitPassVertex���λ�ú�����,����������
	UNITY_TRANSFER_INSTANCE_ID(input, output);

	output.positionWS = TransformObjectToWorld(input.positionOS);
	output.positionCS = TransformWorldToHClip(output.positionWS);
	// ��������ռ�ķ���
	output.normalWS = TransformObjectToWorldNormal(input.normalOS);

	//�������ź�ƫ�ƺ��UV����
	float4 baseST = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _BaseMap_ST);
	output.baseUV = input.baseUV * baseST.xy + baseST.zw;

	return output;
}


float4 LitFra(Varyings input) : SV_TARGET
{
	UNITY_SETUP_INSTANCE_ID(input);
    float4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.baseUV);
	// ͨ��UNITY_ACCESS_INSTANCED_PROP����material����
	float4 baseColor = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _BaseColor);
	float4 base = baseMap * baseColor;
#if defined(_CLIPPING)
	//͸���ȵ�����ֵ��ƬԪ��������
	clip(base.a - UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _Cutoff));
#endif
	//����һ��surface���������
	Surface surface;
	surface.position = input.positionWS;
	surface.normal = normalize(input.normalWS);
	//�õ��ӽǷ���
	surface.viewDirection = normalize(_WorldSpaceCameraPos - input.positionWS);
	surface.color = base.rgb;
	surface.alpha = base.a;
	surface.metallic = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _Metallic);
	surface.smoothness =UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _Smoothness);

#if defined(_PREMULTIPLY_ALPHA)
	BRDF brdf = GetBRDF(surface, true);
#else
	BRDF brdf = GetBRDF(surface);
#endif

	float3 color = GetLighting(surface, brdf);

	return float4(color, surface.alpha);
}

#endif