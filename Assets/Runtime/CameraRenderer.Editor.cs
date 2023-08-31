using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

public partial class CameraRenderer
{
    partial void DrawUnsupportedShaders();
    // ���Ƹ����߿�
    partial void DrawGizmos();
    // ʹ������������û���������
    partial void PrepareBuffer();

#if UNITY_EDITOR
    //SRP��֧�ֵ���ɫ����ǩ����
    static ShaderTagId[] legacyShaderTagIds = {
        new ShaderTagId("Always"),
        new ShaderTagId("ForwardBase"),
        new ShaderTagId("PrepassBase"),
        new ShaderTagId("Vertex"),
        new ShaderTagId("VertexLMRGBM"),
        new ShaderTagId("VertexLM"),
    };
    //���Ƴ�ʹ�ô�����ʵķۺ���ɫ
    static Material errorMaterial;

    string SampleName { get; set; }

    partial void DrawUnsupportedShaders()
    {
        //��֧�ֵ�shaderTag��������ʹ�ô������ר��shader����Ⱦ(��ɫ��ɫ)
        if (errorMaterial == null)
        {
            errorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
        }

        //�����һ��Ԫ����������DrawingSettings��ʱ������
        var drawingSettings = new DrawingSettings(legacyShaderTagIds[0], new SortingSettings(camera))
        { overrideMaterial = errorMaterial };
        for (int i = 1; i < legacyShaderTagIds.Length; i++)
        {
            //�����������������ɫ����PassName����i=1��ʼ
            drawingSettings.SetShaderPassName(i, legacyShaderTagIds[i]);
        }
        //ʹ��Ĭ�����ü��ɣ������������Ķ��Ǵ����
        var filteringSettings = FilteringSettings.defaultValue;
        //���Ʋ�֧�ֵ�shaderTag���͵�����
        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
    }

    partial void DrawGizmos()
    {
        if (Handles.ShouldRenderGizmos())
        {
            context.DrawGizmos(camera, GizmoSubset.PreImageEffects);
            context.DrawGizmos(camera, GizmoSubset.PostImageEffects);
        }
    }

    partial void PrepareBuffer()
    {
        //����һ��ֻ���ڱ༭��ģʽ�²ŷ����ڴ�
        Profiler.BeginSample("Editor Only");
        buffer.name = SampleName = camera.name;
        Profiler.EndSample();
    }
#else
	const string SampleName = bufferName;

#endif
}
