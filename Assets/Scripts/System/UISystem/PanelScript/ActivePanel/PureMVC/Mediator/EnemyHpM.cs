using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpM : Mediator
{
    public new static string NAME = "EnemyHpM";
    private Dictionary<int, PanelBase> panelMap;
    public EnemyHpM() : base(EnemyHpM.NAME)
    {
        panelMap = new Dictionary<int, PanelBase>();
    }
    #region �����洢���ݵĸ���
    public void AddPanel(int id,PanelBase panel)
    {
        if(panelMap == null)
        {
            panelMap= new Dictionary<int, PanelBase>();
        }
        if(panelMap.ContainsKey(id))
        {
            Debug.Log("�Ѿ�����id"+id+"�ĵ��˽�����壬�Խ��������и���");
            panelMap[id] = panel;
        }
        else
        {
            panelMap.Add(id,panel);
        }
    }
    public void RemovePanel(int id)
    {
        if(panelMap.ContainsKey((int)id))
        {
            panelMap.Remove(id);
        }
        else
        {
            Debug.Log("������id"+id+"�ĵ��˽������");
        }
    }
    public void ClearAllData()
    {
        panelMap.Clear();
    }
    #endregion

    //��ʼ������

    public override string[] ListNotificationInterests()
    {


        return new string[]{
            PMConst.UpdateEHpToView
        };
    }
    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case PMConst.UpdateEHpToView:
                //��ȡ���
                int targetid = (int)((object[])notification.Body)[0];//����Ӧ��id����
                float targetnum = (float)((object[])notification.Body)[1];//����Ӧid��Ҫ�յ��˺�����
                (panelMap[targetid] as EnemyHpBarPanel).UpdateHp(targetnum);
                break;
            default:
                break;
        }
    }

    public override void OnRemove()
    {
        base.OnRemove();
        ClearAllData();
    }
}
