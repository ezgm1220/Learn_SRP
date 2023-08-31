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

    // 定义的这两个字段用于将灯光发送到GPU对应的属性中
    static int dirLightColorsId = Shader.PropertyToID("_DirectionalLightColor");
    static int dirLightDirectionsId = Shader.PropertyToID("_DirectionalLightDirection");
 
    //初始化设置
    public void Setup(ScriptableRenderContext context)
    {
        buffer.BeginSample(bufferName);
        // 发送数据
        SetupDirectionalLight();
        buffer.EndSample(bufferName);
        context.ExecuteCommandBuffer(buffer);
        buffer.Clear();
    }

    // 将场景主光源的光照颜色和方向传递到GPU
    void SetupDirectionalLight()
    {
        Light light = RenderSettings.sun;

        buffer.SetGlobalVector(dirLightColorsId, light.color.linear * light.intensity);
        buffer.SetGlobalVector(dirLightDirectionsId, -light.transform.forward);
    }

    
}
