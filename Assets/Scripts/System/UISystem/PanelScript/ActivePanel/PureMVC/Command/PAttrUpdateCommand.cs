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
        //ִ��ָ�������Ϊ
        //��ȡ����Ѫ����
        PlayerStateP proxy = PMFacade.MainInstance.RetrieveProxy(PMConst.PlayerStateP) as PlayerStateP;
        proxy.UpdatePlayerAttr((PlayerAttribute)notification.Body);
    }
}
