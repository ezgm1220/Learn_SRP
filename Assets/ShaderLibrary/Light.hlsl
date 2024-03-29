// 灯光数据相关库
#ifndef CUSTOM_LIGHT_INCLUDED
#define CUSTOM_LIGHT_INCLUDED

#define MAX_DIRECTIONAL_LIGHT_COUNT 4
// 多平行光的属性
CBUFFER_START(_CustomLight)
	//float3 _DirectionalLightColor;
	//float3 _DirectionalLightDirection;
	int _DirectionalLightCount;
    float4 _DirectionalLightColors[MAX_DIRECTIONAL_LIGHT_COUNT];
    float4 _DirectionalLightDirections[MAX_DIRECTIONAL_LIGHT_COUNT];
	// 阴影数据
    float4 _DirectionalLightShadowData[MAX_DIRECTIONAL_LIGHT_COUNT];
CBUFFER_END

// 灯光的属性
struct Light {
	// 颜色
	float3 color;
	// 方向
	float3 direction;
	// 光源阴影衰减
	float attenuation;
};

// 获取方向光源的数量
int GetDirectionalLightCount() {
	return _DirectionalLightCount;
}

// 获取方向光的阴影数据
DirectionalShadowData GetDirectionalShadowData(int lightIndex) 
{
     DirectionalShadowData data;
     data.strength = _DirectionalLightShadowData[lightIndex].x;
     data.tileIndex = _DirectionalLightShadowData[lightIndex].y;
     return data;
}

// 获取目标索引定向光的属性
Light GetDirectionalLight (int index, Surface surfaceWS) {
	Light light;
	light.color = _DirectionalLightColors[index].rgb;
	light.direction = _DirectionalLightDirections[index].xyz;
	// 得到阴影数据
    DirectionalShadowData shadowData = GetDirectionalShadowData(index);
    // 得到阴影衰减
    light.attenuation = GetDirectionalShadowAttenuation(shadowData, surfaceWS);
    return light;
}

#endif