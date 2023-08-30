using UnityEngine.Rendering;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using System;

public class M_RenderPipeline : RenderPipeline
{

    CameraRenderer renderer = new CameraRenderer();

    // 使用SRP合批
    public M_RenderPipeline()
    {
        GraphicsSettings.useScriptableRenderPipelineBatching = true;    
    }
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach (Camera camera in cameras)
        {
            renderer.Render(context, camera);
        }

    }
   
}
