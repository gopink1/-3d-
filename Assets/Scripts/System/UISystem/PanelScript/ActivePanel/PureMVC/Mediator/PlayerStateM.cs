using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Core;
using PureMVC.Patterns.Mediator;
using System;
using PureMVC.Interfaces;

public class PlayerStateM : Mediator
{
    public new static string NAME = "PlayerStateM";
    //��ȡ��ViewComponent
    private PlayerStatePanel PlayerStatePanel;

    public PlayerStateM(object viewComponent) : base(PlayerStateM.NAME)
    {
        if(typeof(PlayerStatePanel) == viewComponent.GetType())
        {
            PlayerStatePanel = viewComponent as PlayerStatePanel;
        }
        else
        {
            Debug.Log("��ʼ��PlayerStateM�ű�ʧ�ܣ�����Ľű����Ͳ���PlayerStatePanel");
        }
    }

    //��ʼ��������Ϣ
    public override string[] ListNotificationInterests()
    {
        return new string[] {
        PMConst.UpdatePHpToView,
        PMConst.UpdatePHpTextToView,
        };
    }

    //��Ӧ��Ϣ��Ҫ��������
    public override void HandleNotification(INotification notification)
    {
        //�������õ���Ϣ�б������Ӧ
        switch (notification.Name)
        {
            case PMConst.UpdatePHpToView:
                //����Ҫ���������Ϣ
                PlayerStatePanel.UpdateHp((float)notification.Body);
                break;
            case PMConst.UpdatePHpTextToView:
                //����Ҫ���������Ϣ
                PlayerStatePanel.UpdateHpText(((float[])notification.Body)[0], ((float[])notification.Body)[1]);
                break;
            default:
                break;
        }

    }

}
