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
            Debug.Log("��ʼ��PlayerStateP�ű�ʧ�ܣ�����Ľű����Ͳ���PlayerHealthyControl");
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    public void UpdateHp(float amount)
    {
        // ��ȡ��ǰ����
        var attr = playerHealthyComtrol.GetAttr();

        // ��������ֵ��Ӧ�İٷֱȣ���������ֵ��
        float ratioChange = amount / attr.MaxHp;

        // ���ͱ��ʸ���֪ͨ
        PMFacade.MainInstance.SendNotification(PMConst.UpdatePHpToView, ratioChange);


    }

    public void UpdateMaxHp()
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

    public void UpdatePlayerAttr(PlayerAttribute playerAttribute)
    {
        // ��ȡ��ǰ����
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

        // �����ı���ʾ
        PMFacade.MainInstance.SendNotification(PMConst.UpdatePAttrToText, new object[]{
            playerAttribute, count
        });
    }
}
