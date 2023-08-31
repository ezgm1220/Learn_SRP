//�ƹ�������ؿ�
#ifndef CUSTOM_LIGHT_INCLUDED
#define CUSTOM_LIGHT_INCLUDED

#define MAX_DIRECTIONAL_LIGHT_COUNT 4
// ��ƽ�й������
CBUFFER_START(_CustomLight)
	//float3 _DirectionalLightColor;
	//float3 _DirectionalLightDirection;
	int _DirectionalLightCount;
    float4 _DirectionalLightColors[MAX_DIRECTIONAL_LIGHT_COUNT];
    float4 _DirectionalLightDirections[MAX_DIRECTIONAL_LIGHT_COUNT];
CBUFFER_END

//�ƹ������
struct Light {
	//��ɫ
	float3 color;
	//����
	float3 direction;
};

//��ȡ�����Դ������
int GetDirectionalLightCount() {
	return _DirectionalLightCount;
}


//��ȡĿ����������������
Light GetDirectionalLight (int index) {
	Light light;
	light.color = _DirectionalLightColors[index].rgb;
	light.direction = _DirectionalLightDirections[index].xyz;
	return light;
}

#endif