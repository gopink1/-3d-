using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeapon : IWeapon
{
    protected override void ShowEffet()
    {
        //显示特效
        Debug.Log("显示武器特效");
    }

    protected override void ShowVoice()
    {
        //显示音效
        Debug.Log("显示武器音效");
    }
}
