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

    // ������������ֶ����ڽ��ƹⷢ�͵�GPU��Ӧ��������
    static int dirLightColorsId = Shader.PropertyToID("_DirectionalLightColor");
    static int dirLightDirectionsId = Shader.PropertyToID("_DirectionalLightDirection");
 
    //��ʼ������
    public void Setup(ScriptableRenderContext context)
    {
        buffer.BeginSample(bufferName);
        // ��������
        SetupDirectionalLight();
        buffer.EndSample(bufferName);
        context.ExecuteCommandBuffer(buffer);
        buffer.Clear();
    }

    // ����������Դ�Ĺ�����ɫ�ͷ��򴫵ݵ�GPU
    void SetupDirectionalLight()
    {
        Light light = RenderSettings.sun;

        buffer.SetGlobalVector(dirLightColorsId, light.color.linear * light.intensity);
        buffer.SetGlobalVector(dirLightDirectionsId, -light.transform.forward);
    }

    
}
