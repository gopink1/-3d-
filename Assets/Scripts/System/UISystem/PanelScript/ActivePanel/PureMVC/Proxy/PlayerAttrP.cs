using PureMVC.Patterns.Proxy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttrP : Proxy
{
    public new const string NAME = "PlayerAttrP";

    public PlayerHealthyControl playerHealthyComtrol;

    public PlayerAttrP(object data) : base(PlayerStateP.NAME)
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

    public void UpdatePlayerAttrPanel()
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
}
