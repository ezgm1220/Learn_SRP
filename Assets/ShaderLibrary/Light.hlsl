//�ƹ�������ؿ�
#ifndef CUSTOM_LIGHT_INCLUDED
#define CUSTOM_LIGHT_INCLUDED

CBUFFER_START(_CustomLight)
	float3 _DirectionalLightColor;
	float3 _DirectionalLightDirection;
CBUFFER_END

//�ƹ������
struct Light {
	//��ɫ
	float3 color;
	//����
	float3 direction;
};

//��ȡĿ����������������
Light GetDirectionalLight () 
{
	Light light;
	light.color = _DirectionalLightColor;
	light.direction = _DirectionalLightDirection;

	return light;
}

#endif