// 光照计算相关库
#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

// 得到入射光的数据
float3 IncomingLight (Surface surface, Light light)
{	
	// 将灯光的阴影衰减添加到入射光强度的计算中。
	return saturate(dot(surface.normal, light.direction)* light.attenuation) * light.color;
}

float3 GetLighting (Surface surface, BRDF brdf, Light light)
{
	return IncomingLight(surface, light) * DirectBRDF(surface, brdf, light);
}

float3 GetLighting(Surface surfaceWS, BRDF brdf)
{
	//float3 color = 0.0;
	//for (int i = 0; i < GetDirectionalLightCount(); i++) {
	//	color += GetLighting(surface, brdf, GetDirectionalLight(i));
	//}
	//return color;

	//可见光的光照结果进行累加得到最终光照结果
    float3 color = 0.0;
    for (int i = 0; i < GetDirectionalLightCount(); i++) 
    {
        color += GetLighting(surfaceWS, brdf, GetDirectionalLight(i, surfaceWS));
    }
    return color;
}


#endif
