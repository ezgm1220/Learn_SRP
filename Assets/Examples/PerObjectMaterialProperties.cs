using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �ҵ�ͬ���ʶ����ϣ�����Ϊÿ���������ò�ͬ������
/// </summary>
[DisallowMultipleComponent]
public class PerObjectMaterialProperties : MonoBehaviour
{
    static int baseColorId = Shader.PropertyToID("_BaseColor");
    //static int cutoffId = Shader.PropertyToID("_Cutoff");

    [SerializeField]
    Color baseColor = Color.white;
    //[SerializeField, Range(0f, 1f)]
    //float cutoff = 0.5f;

    static MaterialPropertyBlock block;

    void OnValidate()
    {
        if (block == null)
        {
            block = new MaterialPropertyBlock();
        }
        //���ò�������
        block.SetColor(baseColorId, baseColor);
        //block.SetFloat(cutoffId, cutoff);
        GetComponent<Renderer>().SetPropertyBlock(block);
    }
    void Awake()
    {
        OnValidate();
    }
}
