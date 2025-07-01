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
        //执行boss姓名更改
        //获取代理
        BossHpP proxy = PMFacade.MainInstance.RetrieveProxy(PMConst.BossHpP) as BossHpP;//获取代理
        proxy.SetBoss((EnemyBossHealthyControl)notification.Body);
    }

}
