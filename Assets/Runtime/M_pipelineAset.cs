using UnityEngine.Rendering;
using UnityEngine;

[CreateAssetMenu(menuName = "Rendering/M_RenderPipeline")]
public class CustomRenderPipelineAsset : RenderPipelineAsset
{

    //设置批处理启用状态
    [SerializeField]
    bool useDynamicBatching = true, useGPUInstancing = true, useSRPBatcher = true;
    //阴影配置
    [SerializeField]
    ShadowSettings shadows = default;

    protected override RenderPipeline CreatePipeline()
    {
        return new M_RenderPipeline(useDynamicBatching, useGPUInstancing, useSRPBatcher, shadows);
    }


}