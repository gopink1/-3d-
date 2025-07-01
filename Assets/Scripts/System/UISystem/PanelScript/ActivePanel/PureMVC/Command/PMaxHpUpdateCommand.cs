using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMaxHpUpdateCommand : MacroCommand
{
    public override void Execute(INotification notification)
    {
        base.Execute(notification);
        //ִ��ָ�������Ϊ
        //��ȡ����Ѫ����
        PlayerStateP proxy = PMFacade.MainInstance.RetrieveProxy(PMConst.PlayerStateP) as PlayerStateP;
        Debug.Log(notification.Body);
        //ִ�п�Ѫ�����п�Ѫ�Ĳ���
        proxy.UpdateMaxHp();
    }
}
