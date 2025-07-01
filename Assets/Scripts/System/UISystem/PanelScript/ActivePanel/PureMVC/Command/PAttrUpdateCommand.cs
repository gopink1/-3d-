using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PAttrUpdateCommand : MacroCommand
{
    public override void Execute(INotification notification)
    {
        base.Execute(notification);
        //执行指令相关行为
        //获取到扣血代理
        PlayerStateP proxy = PMFacade.MainInstance.RetrieveProxy(PMConst.PlayerStateP) as PlayerStateP;
        proxy.UpdatePlayerAttr((PlayerAttribute)notification.Body);
    }
}
