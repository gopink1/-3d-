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
        //�˺����ʸı䣬������Ҫ�ı�Ķ�������conbat����Ĺ������ʸı�
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
