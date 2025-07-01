using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDamageChangeEffect : ItemEffectBase, IDamageChangeEffect
{

    float upValue;
    public PDamageChangeEffect(float upValue)
    {
        this.upValue=upValue;
    }

    public void AppayEffect()
    {
        //伤害倍率改变，传入需要改变的对象，让其conbat对象的攻击倍率改变
        float f= GameBase.MainInstance.GetMainPlayer().GetComponent<PlayerCombatControl>().AtkMultiplier;
        GameBase.MainInstance.GetMainPlayer().GetComponent<PlayerCombatControl>().AtkMultiplier = f * (upValue + 1 );
    }
    public override void RegistEvent()
    {
        AppayEffect();
    }

    public override void RemoveEvent()
    {
        throw new System.NotImplementedException();
    }
}
