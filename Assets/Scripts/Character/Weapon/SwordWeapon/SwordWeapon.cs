using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeapon : IWeapon
{
    protected override void ShowEffet()
    {
        //��ʾ��Ч
        Debug.Log("��ʾ������Ч");
    }

    protected override void ShowVoice()
    {
        //��ʾ��Ч
        Debug.Log("��ʾ������Ч");
    }
}
