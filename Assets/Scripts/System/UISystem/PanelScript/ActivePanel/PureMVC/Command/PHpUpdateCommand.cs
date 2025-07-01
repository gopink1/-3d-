using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;

public class PHpUpdateCommand : MacroCommand
{
    public override void Execute(INotification notification)
    {
        base.Execute(notification);
        //执行指令相关行为
        //获取到扣血代理
        PlayerStateP proxy =  PMFacade.MainInstance.RetrieveProxy(PMConst.PlayerStateP) as PlayerStateP;
        Debug.Log(notification.Body);
        //执行扣血代理中扣血的操作
        proxy.UpdateHp((float)notification.Body);
    }
}
