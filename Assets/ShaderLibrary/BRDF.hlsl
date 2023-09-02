//BRDF��ؿ�
#ifndef CUSTOM_BRDF_INCLUDED
#define CUSTOM_BRDF_INCLUDED

//BRDF����
struct BRDF {
    //������
	float3 diffuse;
	//���淴��
	float3 specular;
	//�ֲڶ�
	float roughness;
};

//����ʵķ�����ƽ��Լ0.04
#define MIN_REFLECTIVITY 0.04
//���㲻�����ֵ������Χ�� 0-1 ������ 0-0.96�����ֺ�URP��һ��
float OneMinusReflectivity (float metallic) {
	float range = 1.0 - MIN_REFLECTIVITY;
	return range - metallic * range;
}

//�õ������BRDF����
BRDF GetBRDF (Surface surface, bool applyAlphaToDiffuse = false) {

	BRDF brdf;

	float oneMinusReflectivity = OneMinusReflectivity(surface.metallic);

	brdf.diffuse = surface.color * oneMinusReflectivity;
	brdf.specular = lerp(MIN_REFLECTIVITY, surface.color, surface.metallic);

	//�⻬��תΪʵ�ʴֲڶ�
	float perceptualRoughness = PerceptualSmoothnessToPerceptualRoughness(surface.smoothness);
	brdf.roughness = PerceptualRoughnessToRoughness(perceptualRoughness);

	return brdf;
	//BRDF brdf;
	//float oneMinusReflectivity = OneMinusReflectivity(surface.metallic);
	//brdf.diffuse = surface.color * oneMinusReflectivity;
	////͸����Ԥ��
	//if (applyAlphaToDiffuse) {
	//	brdf.diffuse *= surface.alpha;
	//}
	//brdf.specular = lerp(MIN_REFLECTIVITY, surface.color, surface.metallic);
	////�⻬��תΪʵ�ʴֲڶ�
	//float perceptualRoughness = PerceptualSmoothnessToPerceptualRoughness(surface.smoothness);
	//brdf.roughness = PerceptualRoughnessToRoughness(perceptualRoughness);
	//return brdf;
}
//���ݹ�ʽ�õ����淴��ǿ��
float SpecularStrength (Surface surface, BRDF brdf, Light light) {
	float3 h = SafeNormalize(light.direction + surface.viewDirection);
	float nh2 = Square(saturate(dot(surface.normal, h)));
	float lh2 = Square(saturate(dot(light.direction, h)));
	float r2 = Square(brdf.roughness);
	float d2 = Square(nh2 * (r2 - 1.0) + 1.00001);
	float normalization = brdf.roughness * 4.0 + 2.0;
	return r2 / (d2 * max(0.1, lh2) * normalization);
}
//��ȡ����BRDF��ֱ������
float3 DirectBRDF (Surface surface, BRDF brdf, Light light) 
{
	return SpecularStrength(surface, brdf, light) * brdf.specular + brdf.diffuse;
}

#endif
