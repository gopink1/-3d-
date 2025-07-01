using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHpUpdateCommand : MacroCommand
{
    public override void Execute(INotification notification)
    {
        base.Execute(notification);
        //执行command
        BossHpP proxy = PMFacade.MainInstance.RetrieveProxy(PMConst.BossHpP) as BossHpP;//获取代理
        proxy.UpdateHp((float)notification.Body);
    }
}
