using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttrM : Mediator
{
    public new static string NAME = "PlayerAttrM";
    //获取到ViewComponent
    private PlayingPanel PlayingPanel;
    public PlayerAttrM(object viewComponent) : base(PlayerStateM.NAME)
    {
        if (typeof(PlayingPanel) == viewComponent.GetType())
        {
            PlayingPanel = viewComponent as PlayingPanel;
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
        PMConst.UpdatePAttrToText,
        };
    }

    //响应消息需要处理事务
    public override void HandleNotification(INotification notification)
    {
        //根据配置的消息列表进行响应
        switch (notification.Name)
        {
            case PMConst.UpdatePAttrToText:
                //当需要更新面板信息
                PlayingPanel.UpdatePlayerAttr((PlayerAttribute)((object[])notification.Body)[0], (float)((object[])notification.Body)[1]);
                break;
            default:
                break;
        }

    }
}
