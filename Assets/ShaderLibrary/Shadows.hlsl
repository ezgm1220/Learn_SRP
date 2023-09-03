//��Ӱ����
#ifndef CUSTOM_SHADOWS_INCLUDED
#define CUSTOM_SHADOWS_INCLUDED
 
#define MAX_SHADOWED_DIRECTIONAL_LIGHT_COUNT 4
//��Ӱͼ��
TEXTURE2D_SHADOW(_DirectionalShadowAtlas);
#define SHADOW_SAMPLER sampler_linear_clamp_compare
SAMPLER_CMP(SHADOW_SAMPLER);
 
CBUFFER_START(_CustomShadows)
// ��Ӱת������
float4x4 _DirectionalShadowMatrices[MAX_SHADOWED_DIRECTIONAL_LIGHT_COUNT];
CBUFFER_END
 
// ��Ӱ��������Ϣ
struct DirectionalShadowData 
{
   float strength;
   int tileIndex;
};

// ������Ӱͼ��
float SampleDirectionalShadowAtlas(float3 positionSTS) 
{
    return SAMPLE_TEXTURE2D_SHADOW(_DirectionalShadowAtlas, SHADOW_SAMPLER, positionSTS);
}

// ������Ӱ˥��
float GetDirectionalShadowAttenuation(DirectionalShadowData data, Surface surfaceWS) 
{
    // ����Ӱǿ��Ϊ��ʱû��˥��(˥��ֵΪ1)
    if (data.strength <= 0.0) 
    {
        return 1.0;
    }

    // ͨ����Ӱת������ͱ���λ�õõ�����Ӱ����(ͼ��)�ռ��λ�ã�Ȼ���ͼ�����в���
    float3 positionSTS = mul(_DirectionalShadowMatrices[data.tileIndex],float4(surfaceWS.position, 1.0)).xyz;
    float shadow = SampleDirectionalShadowAtlas(positionSTS);

    // ����Ӱ˥��ֵ����Ӱǿ�Ⱥ�˥�����ӵĲ�ֵ
    return lerp(1.0, shadow, data.strength);
}

#endif