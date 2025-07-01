using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHpM : Mediator
{
    public new static string NAME = "BossHpM";
    private BossHpBarPanel m_panel;
    public BossHpM(object viewComponent) : base(BossHpM.NAME)
    {
        if (typeof(BossHpBarPanel) == viewComponent.GetType())
        {
            m_panel = viewComponent as BossHpBarPanel;
        }
        else
        {
            Debug.Log("初始化PlayerStateM脚本失败，传入的脚本类型不是PlayerStatePanel");
        }
    }

    //注册该中介者监听的事件
    public override string[] ListNotificationInterests()
    {

        return new string[] { 
        PMConst.UpdateBossHpToView,
        PMConst.UpdateBossNameToView
        };
    }
    //当事件发生触发相应的响应
    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case PMConst.UpdateBossHpToView:
                //更新血条
                m_panel.UpdateHp((float)notification.Body);
                break;
            case PMConst.UpdateBossNameToView:
                //更新boss
                //更新boss然后把血条设置为指定血量
                EnemyBossHealthyControl hpControl = notification.Body as EnemyBossHealthyControl;
                string orName = hpControl.gameObject.name;
                string bossName = orName.Replace("(Clone)", "");
                m_panel.SetBoss(bossName, (float)(hpControl.GetAttr().GetCurrentHp()/hpControl.GetAttr().MaxHP));//设置boss

                break;
            default:
                break;
        }

    }


}
