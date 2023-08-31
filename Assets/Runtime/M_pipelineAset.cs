using UnityEngine.Rendering;
using UnityEngine;

[CreateAssetMenu(menuName = "Rendering/M_RenderPipeline")]
public class CustomRenderPipelineAsset : RenderPipelineAsset
{

    //��������������״̬
    [SerializeField]
    bool useDynamicBatching = true, useGPUInstancing = true, useSRPBatcher = true;

    protected override RenderPipeline CreatePipeline()
    {
        return new M_RenderPipeline(useDynamicBatching, useGPUInstancing, useSRPBatcher);
    }
}