using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickToUpAttrEffect : ItemEffectBase, IPickUpEffect
{
    private PlayerAttribute playerAttribute = PlayerAttribute.MaxHP;
    private float value;

    public PickToUpAttrEffect(PlayerAttribute playerAttribute,float value)
    {
        this.playerAttribute = playerAttribute;
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

    public override void RegistEvent()
    {
        OnPickUp();
    }

    public override void RemoveEvent()
    {
        OnRemove();
    }
}
