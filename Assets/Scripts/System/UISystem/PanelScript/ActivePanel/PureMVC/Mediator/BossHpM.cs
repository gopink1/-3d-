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
            Debug.Log("��ʼ��PlayerStateM�ű�ʧ�ܣ�����Ľű����Ͳ���PlayerStatePanel");
        }
    }

    //ע����н��߼������¼�
    public override string[] ListNotificationInterests()
    {

        return new string[] { 
        PMConst.UpdateBossHpToView,
        PMConst.UpdateBossNameToView
        };
    }
    //���¼�����������Ӧ����Ӧ
    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case PMConst.UpdateBossHpToView:
                //����Ѫ��
                m_panel.UpdateHp((float)notification.Body);
                break;
            case PMConst.UpdateBossNameToView:
                //����boss
                //����bossȻ���Ѫ������Ϊָ��Ѫ��
                EnemyBossHealthyControl hpControl = notification.Body as EnemyBossHealthyControl;
                string orName = hpControl.gameObject.name;
                string bossName = orName.Replace("(Clone)", "");
                m_panel.SetBoss(bossName, (float)(hpControl.GetAttr().GetCurrentHp()/hpControl.GetAttr().MaxHP));//����boss

                break;
            default:
                break;
        }

    }


}
