using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttrM : Mediator
{
    public new static string NAME = "PlayerAttrM";
    //��ȡ��ViewComponent
    private PlayingPanel PlayingPanel;
    public PlayerAttrM(object viewComponent) : base(PlayerStateM.NAME)
    {
        if (typeof(PlayingPanel) == viewComponent.GetType())
        {
            PlayingPanel = viewComponent as PlayingPanel;
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
        PMConst.UpdatePAttrToText,
        };
    }

    //��Ӧ��Ϣ��Ҫ��������
    public override void HandleNotification(INotification notification)
    {
        //�������õ���Ϣ�б������Ӧ
        switch (notification.Name)
        {
            case PMConst.UpdatePAttrToText:
                //����Ҫ���������Ϣ
                PlayingPanel.UpdatePlayerAttr((PlayerAttribute)((object[])notification.Body)[0], (float)((object[])notification.Body)[1]);
                break;
            default:
                break;
        }

    }
}
