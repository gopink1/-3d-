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
        //ִ��ָ�������Ϊ
        //��ȡ����Ѫ����
        PlayerStateP proxy =  PMFacade.MainInstance.RetrieveProxy(PMConst.PlayerStateP) as PlayerStateP;
        Debug.Log(notification.Body);
        //ִ�п�Ѫ�����п�Ѫ�Ĳ���
        proxy.UpdateHp((float)notification.Body);
    }
}
