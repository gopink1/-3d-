using PureMVC.Patterns.Proxy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHpP : Proxy
{
    public new const string NAME = "BossHpP";
    private EnemyBossHealthyControl m_healthycfg;
    //�������ݲ����boss������
    public BossHpP() : base(NAME)
    {
        //����ʼ�����˵Ľ����ű�
    }

    public void SetBoss(EnemyBossHealthyControl healthycfg)
    {
        m_healthycfg = healthycfg;
        PMFacade.MainInstance.SendNotification(PMConst.UpdateBossNameToView, m_healthycfg);
    }

    /// <summary>
    /// ����Ѫ��
    /// </summary>
    public void UpdateHp(float amount)
    {
        //�������
        float radio = (float)(amount / m_healthycfg.GetAttr().MaxHP);
        PMFacade.MainInstance.SendNotification(PMConst.UpdateBossHpToView, radio);
    }
}
