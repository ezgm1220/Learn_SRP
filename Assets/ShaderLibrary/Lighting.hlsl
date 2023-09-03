// ���ռ�����ؿ�
#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

// �õ�����������
float3 IncomingLight (Surface surface, Light light)
{	
	// ���ƹ����Ӱ˥����ӵ������ǿ�ȵļ����С�
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

	//�ɼ���Ĺ��ս�������ۼӵõ����չ��ս��
    float3 color = 0.0;
    for (int i = 0; i < GetDirectionalLightCount(); i++) 
    {
        color += GetLighting(surfaceWS, brdf, GetDirectionalLight(i, surfaceWS));
    }
    return color;
}


#endif
