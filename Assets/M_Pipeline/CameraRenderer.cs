using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraRenderer
{
    ScriptableRenderContext context;

    Camera camera;

    const string bufferName = "Render Camera";

    CommandBuffer buffer = new CommandBuffer
    {
        name = bufferName
    };

    public void Render(ScriptableRenderContext context, Camera camera)
    {
        this.context = context;
        this.camera = camera;

        Setup();

        if (!Cull())
        {
            return;
        }

        DrawVisibleGeometry();

        Submit();
    }

    CullingResults cullingResults;
    static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");


    bool Cull()
    {
        ScriptableCullingParameters p;

        if(camera.TryGetCullingParameters(out p))
        {
            cullingResults = context.Cull(ref p);
            return true;
        }
        return false;   
    }


    void Setup()
    {
        context.SetupCameraProperties(camera);
        buffer.ClearRenderTarget(true, true, Color.clear);
        buffer.BeginSample(bufferName);
        ExecuteBuffer();
    }

    void DrawVisibleGeometry()
    {
        // 设置绘制顺序和指定渲染相机
        var sortingSettings = new SortingSettings(camera)
        {
            criteria = SortingCriteria.CommonOpaque
        };
        
        // 设置渲染的Shader Pass和排序模式
        var drawingSettings = new DrawingSettings(unlitShaderTagId,sortingSettings);
        
        // 只绘制 RebderQueue 为 opaque 不透明的物体
        var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);

        // 绘制不透明物体
        context.DrawRenderers(
            cullingResults, ref drawingSettings, ref filteringSettings
        );

        context.DrawSkybox(camera);

        sortingSettings.criteria = SortingCriteria.CommonTransparent;
        drawingSettings.sortingSettings = sortingSettings;
        // 只绘制透明物体
        filteringSettings.renderQueueRange = RenderQueueRange.transparent;
        // 绘制透明物体
        context.DrawRenderers(
           cullingResults, ref drawingSettings, ref filteringSettings
       );
    }

    void Submit()
    {
        buffer.EndSample(bufferName);
        ExecuteBuffer();
        context.Submit();
    }

    void ExecuteBuffer()
    {
        context.ExecuteCommandBuffer(buffer);
        buffer.Clear(); 
    }
}