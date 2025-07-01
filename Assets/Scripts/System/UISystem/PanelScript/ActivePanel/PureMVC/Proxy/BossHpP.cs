using PureMVC.Patterns.Proxy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHpP : Proxy
{
    public new const string NAME = "BossHpP";
    private EnemyBossHealthyControl m_healthycfg;
    //链接数据层关联boss的数据
    public BossHpP() : base(NAME)
    {
        //不初始化敌人的健康脚本
    }

    public void SetBoss(EnemyBossHealthyControl healthycfg)
    {
        m_healthycfg = healthycfg;
        PMFacade.MainInstance.SendNotification(PMConst.UpdateBossNameToView, m_healthycfg);
    }

    /// <summary>
    /// 减少血量
    /// </summary>
    public void UpdateHp(float amount)
    {
        //计算比率
        float radio = (float)(amount / m_healthycfg.GetAttr().MaxHP);
        PMFacade.MainInstance.SendNotification(PMConst.UpdateBossHpToView, radio);
    }
}
