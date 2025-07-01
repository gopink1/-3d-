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
    #region 对面板存储数据的更改
    public void AddPanel(int id,PanelBase panel)
    {
        if(panelMap == null)
        {
            panelMap= new Dictionary<int, PanelBase>();
        }
        if(panelMap.ContainsKey(id))
        {
            Debug.Log("已经存在id"+id+"的敌人健康面板，对健康面板进行更新");
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
            Debug.Log("不存在id"+id+"的敌人健康面板");
        }
    }
    public void ClearAllData()
    {
        panelMap.Clear();
    }
    #endregion

    //初始化监听

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
                //获取面板
                int targetid = (int)((object[])notification.Body)[0];//将对应的id拆箱
                float targetnum = (float)((object[])notification.Body)[1];//将对应id需要收到伤害拆箱
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
