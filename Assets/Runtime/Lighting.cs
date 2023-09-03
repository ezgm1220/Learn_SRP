using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Lighting
{

    const string bufferName = "Lighting";

    CommandBuffer buffer = new CommandBuffer
    {
        name = bufferName
    };

    // �洢����޳���Ľ��
    CullingResults cullingResults;

    //�������ɼ����������
    const int maxDirLightCount = 4;

    // �������ݵ��ֶ�
    static int dirLightCountId = Shader.PropertyToID("_DirectionalLightCount");
    static int dirLightColorsId = Shader.PropertyToID("_DirectionalLightColors");
    static int dirLightDirectionsId = Shader.PropertyToID("_DirectionalLightDirections");
    // �洢��������ɫ�ͷ���
    static Vector4[] dirLightColors = new Vector4[maxDirLightCount];
    static Vector4[] dirLightDirections = new Vector4[maxDirLightCount];

    // ������Ӱ��
    Shadows shadows = new Shadows();
    static int dirLightShadowDataId = Shader.PropertyToID("_DirectionalLightShadowData");
    //�洢��Ӱ����
    static Vector4[] dirLightShadowData = new Vector4[maxDirLightCount];

    //��ʼ������
    public void Setup(ScriptableRenderContext context, CullingResults cullingResults, ShadowSettings shadowSettings)
    {
        this.cullingResults = cullingResults;
        buffer.BeginSample(bufferName);

        //��Ӱ�ĳ�ʼ������
        shadows.Setup(context, cullingResults, shadowSettings);
        //// ��������
        //SetupDirectionalLight();
        SetupLights();
        // ��Ⱦ��Ӱ
        shadows.Render();

        buffer.EndSample(bufferName);
        context.ExecuteCommandBuffer(buffer);
        buffer.Clear();
    }

    // ����������Դ�Ĺ�����ɫ�ͷ��򴫵ݵ�GPU
    void SetupDirectionalLight(int index, ref VisibleLight visibleLight)
    {
        dirLightColors[index] = visibleLight.finalColor;
        // ͨ��VisibleLight.localToWorldMatrix�����ҵ�ǰ��ʸ��,���ھ�������У���Ҫ����ȡ��
        dirLightDirections[index] = -visibleLight.localToWorldMatrix.GetColumn(2);

        shadows.ReserveDirectionalShadows(visibleLight.light, index);

        //�洢��Ӱ����
        dirLightShadowData[index] = shadows.ReserveDirectionalShadows(visibleLight.light, index);
    }

    void SetupLights()
    {
        // �õ�����Ӱ�������Ⱦ����Ŀɼ�������
        NativeArray<VisibleLight> visibleLights = cullingResults.visibleLights;

        int dirLightCount = 0;
        for (int i = 0; i < visibleLights.Length; i++)
        {
            VisibleLight visibleLight = visibleLights[i];

            if (visibleLight.lightType == LightType.Directional)
            {
                //VisibleLight�ṹ�ܴ�,���Ǹ�Ϊ�������ò��Ǵ���ֵ�������������ɸ���
                SetupDirectionalLight(dirLightCount++, ref visibleLight);
                //�������ƹ�����������ֹѭ��
                if (dirLightCount >= maxDirLightCount)
                {
                    break;
                }
            }
        }

        buffer.SetGlobalInt(dirLightCountId, dirLightCount);
        buffer.SetGlobalVectorArray(dirLightColorsId, dirLightColors);
        buffer.SetGlobalVectorArray(dirLightDirectionsId, dirLightDirections);
        buffer.SetGlobalVectorArray(dirLightShadowDataId, dirLightShadowData);

    }


    //�ͷ������RT�ڴ�
    public void Cleanup()
    {
        shadows.Cleanup();
    }

}
