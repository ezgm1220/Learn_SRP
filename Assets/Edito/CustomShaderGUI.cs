using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
/// <summary>
/// 扩展材质面板
/// </summary>
public class CustomShaderGUI : ShaderGUI
{
    MaterialEditor editor;
    Object[] materials;
    MaterialProperty[] properties;
    bool showPresets;
    public override void OnGUI(
        MaterialEditor materialEditor, MaterialProperty[] properties
    )
    {
        base.OnGUI(materialEditor, properties);
        editor = materialEditor;
        materials = materialEditor.targets;
        this.properties = properties;

        EditorGUILayout.Space();
        showPresets = EditorGUILayout.Foldout(showPresets, "Presets", true);
        if (showPresets)
        {
            OpaquePreset();
            ClipPreset();
            FadePreset();
            TransparentPreset();
        }
    }
    // 设置材质属性
    bool SetProperty(string name, float value)
    {
        MaterialProperty property = FindProperty(name, properties, false);
        if (property != null)
        {
            property.floatValue = value;
            return true;
        }
        return false;
    }
    
    // 设置关键字状态
    void SetKeyword(string keyword, bool enabled)
    {
        if (enabled)
        {
            foreach (Material m in materials)
            {
                m.EnableKeyword(keyword);
            }
        }
        else
        {
            foreach (Material m in materials)
            {
                m.DisableKeyword(keyword);
            }
        }
    }
    
    // 同时设置关键字和属性
    void SetProperty(string name, string keyword, bool value)
    {
        if (SetProperty(name, value ? 1f : 0f))
        {
            SetKeyword(keyword, value);
        }
    }

    // 定义一些属性来设置材质上对应的属性值
    bool Clipping
    {
        set => SetProperty("_Clipping", "_CLIPPING", value);
    }

    bool PremultiplyAlpha
    {
        set => SetProperty("_PremulAlpha", "_PREMULTIPLY_ALPHA", value);
    }

    BlendMode SrcBlend
    {
        set => SetProperty("_SrcBlend", (float)value);
    }

    BlendMode DstBlend
    {
        set => SetProperty("_DstBlend", (float)value);
    }

    bool ZWrite
    {
        set => SetProperty("_ZWrite", value ? 1f : 0f);
    }
    RenderQueue RenderQueue
    {
        set
        {
            foreach (Material m in materials)
            {
                m.renderQueue = (int)value;
            }
        }
    }

    // 给每种渲染模式设置一个按钮
    bool PresetButton(string name)
    {
        if (GUILayout.Button(name))
        {
            // 重置属性
            editor.RegisterPropertyChangeUndo(name);
            return true;
        }
        return false;
    }

    // 不透明材质默认设置
    void OpaquePreset()
    {
        if (PresetButton("Opaque"))
        {
            Clipping = false;
            PremultiplyAlpha = false;
            SrcBlend = BlendMode.One;
            DstBlend = BlendMode.Zero;
            ZWrite = true;
            RenderQueue = RenderQueue.Geometry;
        }
    }

    // 裁切材质默认设置
    void ClipPreset()
    {
        if (PresetButton("Clip"))
        {
            Clipping = true;
            PremultiplyAlpha = false;
            SrcBlend = BlendMode.One;
            DstBlend = BlendMode.Zero;
            ZWrite = true;
            RenderQueue = RenderQueue.AlphaTest;
        }
    }

    // 标准透明材质默认设置
    void FadePreset()
    {
        if (PresetButton("Fade"))
        {
            Clipping = false;
            PremultiplyAlpha = false;
            SrcBlend = BlendMode.SrcAlpha;
            DstBlend = BlendMode.OneMinusSrcAlpha;
            ZWrite = false;
            RenderQueue = RenderQueue.Transparent;
        }
    }
    // 如果shader的预乘属性不存在，不需要显示该渲染模式的按钮
    bool HasProperty(string name) => FindProperty(name, properties, false) != null;
    bool HasPremultiplyAlpha => HasProperty("_PremulAlpha");

    // 受光正确的透明材质默认设置
    void TransparentPreset()
    {
        if (HasPremultiplyAlpha && PresetButton("Transparent"))
        {
            Clipping = false;
            PremultiplyAlpha = true;
            SrcBlend = BlendMode.One;
            DstBlend = BlendMode.OneMinusSrcAlpha;
            ZWrite = false;
            RenderQueue = RenderQueue.Transparent;
        }
    }
}