using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRenderer
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

        PrepareBuffer();

        if (!Cull())
        {
            return;
        }

        Setup();

        DrawVisibleGeometry();

        // �����߿�
        DrawGizmos();

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
        //�õ������clear flags
        CameraClearFlags flags = camera.clearFlags;
        //����������״̬
        buffer.ClearRenderTarget(flags <= CameraClearFlags.Depth, flags == CameraClearFlags.Color,
            flags == CameraClearFlags.Color ? camera.backgroundColor.linear : Color.clear);
        buffer.BeginSample(SampleName);
        ExecuteBuffer();
    }

    void DrawVisibleGeometry()
    {
        // ���û���˳���ָ����Ⱦ���
        var sortingSettings = new SortingSettings(camera)
        {
            criteria = SortingCriteria.CommonOpaque
        };
        
        // ������Ⱦ��Shader Pass������ģʽ
        var drawingSettings = new DrawingSettings(unlitShaderTagId,sortingSettings);
        
        // ֻ���� RebderQueue Ϊ opaque ��͸��������
        var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);

        // ���Ʋ�͸������
        context.DrawRenderers(
            cullingResults, ref drawingSettings, ref filteringSettings
        );

        context.DrawSkybox(camera);

        sortingSettings.criteria = SortingCriteria.CommonTransparent;
        drawingSettings.sortingSettings = sortingSettings;
        // ֻ����͸������
        filteringSettings.renderQueueRange = RenderQueueRange.transparent;
        // ����͸������
        context.DrawRenderers(
           cullingResults, ref drawingSettings, ref filteringSettings
       );
    }

    void Submit()
    {
        buffer.EndSample(SampleName);
        ExecuteBuffer();
        context.Submit();
    }

    void ExecuteBuffer()
    {
        context.ExecuteCommandBuffer(buffer);
        buffer.Clear(); 
    }
}