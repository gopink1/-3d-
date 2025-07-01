using PureMVC.Patterns.Proxy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateP : Proxy
{
    public new const string NAME = "PlayerStateP";

    public PlayerHealthyControl playerHealthyComtrol;

    public PlayerStateP(object data) : base(PlayerStateP.NAME)
    {
        if (typeof(PlayerHealthyControl) == data.GetType())
        {
            playerHealthyComtrol = data as PlayerHealthyControl;
        }
        else
        {
            Debug.Log("初始化PlayerStateP脚本失败，传入的脚本类型不是PlayerHealthyControl");
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void UpdateHp(float amount)
    {
        // 获取当前属性
        var attr = playerHealthyComtrol.GetAttr();

        // 计算增减值对应的百分比（相对于最大值）
        float ratioChange = amount / attr.MaxHp;

        // 发送比率更新通知
        PMFacade.MainInstance.SendNotification(PMConst.UpdatePHpToView, ratioChange);


    }

    public void UpdateMaxHp()
    {
        // 获取当前属性
        var attr = playerHealthyComtrol.GetAttr();

        // 更新文本显示
        PMFacade.MainInstance.SendNotification(PMConst.UpdatePHpTextToView, new float[]
        {
            attr.CurrentHp,
            attr.MaxHp
        });
    }

    public void UpdatePlayerAttr(PlayerAttribute playerAttribute)
    {
        // 获取当前属性
        var attr = playerHealthyComtrol.GetAttr();
        float count = 0.0f;
        switch (playerAttribute)
        {
            case PlayerAttribute.MaxHP:
                count = attr.MaxHp;
                break;
            case PlayerAttribute.MaxMP:
                count = attr.MaxMp;
                break;
            case PlayerAttribute.ATK:
                count = attr.Atk;
                break;
            case PlayerAttribute.DEF:
                count = attr.Def;
                break;
            case PlayerAttribute.SPEED:
                count = attr.Speed;
                break;
            default:
                break;
        }

        // 更新文本显示
        PMFacade.MainInstance.SendNotification(PMConst.UpdatePAttrToText, new object[]{
            playerAttribute, count
        });
    }
}
