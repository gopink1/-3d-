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
    //获取到ViewComponent
    private PlayerStatePanel PlayerStatePanel;

    public PlayerStateM(object viewComponent) : base(PlayerStateM.NAME)
    {
        if(typeof(PlayerStatePanel) == viewComponent.GetType())
        {
            PlayerStatePanel = viewComponent as PlayerStatePanel;
        }
        else
        {
            Debug.Log("初始化PlayerStateM脚本失败，传入的脚本类型不是PlayerStatePanel");
        }
    }

    //初始化监听消息
    public override string[] ListNotificationInterests()
    {
        return new string[] {
        PMConst.UpdatePHpToView,
        PMConst.UpdatePHpTextToView,
        };
    }

    //响应消息需要处理事务
    public override void HandleNotification(INotification notification)
    {
        //根据配置的消息列表进行响应
        switch (notification.Name)
        {
            case PMConst.UpdatePHpToView:
                //当需要更新面板信息
                PlayerStatePanel.UpdateHp((float)notification.Body);
                break;
            case PMConst.UpdatePHpTextToView:
                //当需要更新面板信息
                PlayerStatePanel.UpdateHpText(((float[])notification.Body)[0], ((float[])notification.Body)[1]);
                break;
            default:
                break;
        }

    }

}
