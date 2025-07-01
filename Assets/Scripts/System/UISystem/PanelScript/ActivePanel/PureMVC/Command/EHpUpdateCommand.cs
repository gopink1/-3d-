using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHpUpdateCommand : MacroCommand
{
    public override void Execute(INotification notification)
    {
        base.Execute(notification);
        EnemyHpP proxy = PMFacade.MainInstance.RetrieveProxy(PMConst.EnemyHpP) as EnemyHpP;
        proxy.UpdateOneEnemyHp((int)((object[])notification.Body)[0], (float)((object[])notification.Body)[1]);
    }
}
