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

    Lighting lighting = new Lighting();

    public void Render(ScriptableRenderContext context, Camera camera, 
        bool useDynamicBatching, bool useGPUInstancing, ShadowSettings shadowSettings)
    {
        this.context = context;
        this.camera = camera;

        PrepareBuffer();

        if (!Cull(shadowSettings.maxDistance))
        {
            return;
        }
        
        buffer.BeginSample(SampleName);
        ExecuteBuffer();

        //��Դ���ݺ���Ӱ���ݷ��͵�GPU�������
        lighting.Setup(context, cullingResults, shadowSettings);
        buffer.EndSample(SampleName);

        Setup();

        // ���Ƽ�����
        DrawVisibleGeometry(useDynamicBatching, useGPUInstancing);
        // �����߿�
        DrawGizmos();
        // �ͷ��������Ӱ��ͼRT���ڴ�
        lighting.Cleanup();

        Submit();
    }

    CullingResults cullingResults;
    static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");
    static ShaderTagId litShaderTagId = new ShaderTagId("CustomLit");

    bool Cull(float maxShadowDistance)
    {
        if (camera.TryGetCullingParameters(out ScriptableCullingParameters p))
        {
            //�õ������Ӱ����,�����Զ�������Ƚϣ�ȡ��С���Ǹ���Ϊ��Ӱ����
            p.shadowDistance = Mathf.Min(maxShadowDistance, camera.farClipPlane);
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

    void DrawVisibleGeometry(bool useDynamicBatching, bool useGPUInstancing)
    {
        // ���û���˳���ָ����Ⱦ���
        var sortingSettings = new SortingSettings(camera)
        {
            criteria = SortingCriteria.CommonOpaque
        };

        // ������Ⱦ��Shader Pass������ģʽ
        var drawingSettings = new DrawingSettings(unlitShaderTagId, sortingSettings)
        {
            //������Ⱦʱ�������ʹ��״̬
            enableDynamicBatching = useDynamicBatching,
            enableInstancing = useGPUInstancing
        };

        // ��ȾCustomLit��ʾ��pass��
        drawingSettings.SetShaderPassName(1, litShaderTagId);

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