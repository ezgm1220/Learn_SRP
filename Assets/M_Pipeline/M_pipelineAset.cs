using UnityEngine.Rendering;
using UnityEngine;

[CreateAssetMenu(menuName = "Rendering/M_RenderPipeline")]
public class CustomRenderPipelineAsset : RenderPipelineAsset
{
    protected override RenderPipeline CreatePipeline()
    {
        return new M_RenderPipeline();
    }
}