using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using PureMVC.Patterns.Proxy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNameUpdateCommand : MacroCommand
{
    public override void Execute(INotification notification)
    {
        base.Execute(notification);
        //ִ��boss��������
        //��ȡ����
        BossHpP proxy = PMFacade.MainInstance.RetrieveProxy(PMConst.BossHpP) as BossHpP;//��ȡ����
        proxy.SetBoss((EnemyBossHealthyControl)notification.Body);
    }

}
