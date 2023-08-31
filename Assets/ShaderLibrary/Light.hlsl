//灯光数据相关库
#ifndef CUSTOM_LIGHT_INCLUDED
#define CUSTOM_LIGHT_INCLUDED

CBUFFER_START(_CustomLight)
	float3 _DirectionalLightColor;
	float3 _DirectionalLightDirection;
CBUFFER_END

//灯光的属性
struct Light {
	//颜色
	float3 color;
	//方向
	float3 direction;
};

//获取目标索引定向光的属性
Light GetDirectionalLight () 
{
	Light light;
	light.color = _DirectionalLightColor;
	light.direction = _DirectionalLightDirection;

	return light;
}

#endif