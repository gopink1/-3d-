using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBase : MonoBehaviour 
{
    //���еĽ���Ļ����������н���Ԥ�Ƽ��ĸ���ű�
    //ͳһ���ǵ���Ϊ

    protected bool isClose;//�����Ƿ�ر�

    protected string panelName;//��������
    public virtual void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public virtual void OpenPanel(string name)
    {

        panelName = name;
        isClose = false;
        SetActive(true);
    }
    public virtual void ClosePanel()
    {
        if (isClose) { return; }
        isClose = true;
        gameObject.SetActive(false);
        Destroy(gameObject);
        if (UIManager.MainInstance.panelDict.ContainsKey(panelName))
        {
            //���������ɾ��
            UIManager.MainInstance.panelDict.Remove(panelName);
        }
    }
}
