using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpEffect : IPickUpEffect
{
    private PlayerAttribute playerAttribute = PlayerAttribute.SPEED;
    private float value;

    public SpeedUpEffect(float value)
    {
        this.value = value;
    }
    public void ChangeAttr()
    {
        //根据传入的数据进行
        GameBase.MainInstance.ChangePlayerAttr(playerAttribute, value);
    }
    public void OnPickUp()
    {
        ChangeAttr();
    }

    public void OnRemove()
    {
        GameBase.MainInstance.ChangePlayerAttr(playerAttribute, -value);
    }
}
