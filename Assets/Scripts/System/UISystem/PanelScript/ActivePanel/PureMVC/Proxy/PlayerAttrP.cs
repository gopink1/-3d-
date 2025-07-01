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
            Debug.Log("��ʼ��PlayerStateP�ű�ʧ�ܣ�����Ľű����Ͳ���PlayerHealthyControl");
        }
    }

    public void UpdatePlayerAttrPanel()
    {
        // ��ȡ��ǰ����
        var attr = playerHealthyComtrol.GetAttr();

        // �����ı���ʾ
        PMFacade.MainInstance.SendNotification(PMConst.UpdatePHpTextToView, new float[]
        {
            attr.CurrentHp,
            attr.MaxHp
        });
    }
}
