using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILayer
{
    private int curLayerIndex = 50;
    public void ResetLayer()
    {
        curLayerIndex = 50;
    }

    /// <summary>
    /// ����ҳ��㼶
    /// </summary>
    /// <param name="obj">��Ҫ���ò㼶��ҳ������</param>
    public void SetLayer(GameObject obj)
    {
        //��ȡ���
        Canvas[] canvas = obj.GetComponentsInChildren<Canvas>();
        if (canvas != null)
        {
            //�����е�UI���ò㼶
            for (int i = 0; i < canvas.Length; i++)
            {
                canvas[i].sortingOrder += curLayerIndex;
            }
        }
        curLayerIndex++;
    }
}
