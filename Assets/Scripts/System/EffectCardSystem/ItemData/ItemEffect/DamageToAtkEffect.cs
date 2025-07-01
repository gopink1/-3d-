using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageToAtkEffect : ItemEffectBase, IDamageTakenEffect
{
    public void OnDamageTaken(EffectEventArgs args)
    {
        //每当收到伤害就会增加攻击力
    }

    public override void RegistEvent()
    {
        GameEventManager.MainInstance.AddEventListening<EffectEventArgs>(EventHash.DamageTaken, OnDamageTaken);
    }

    public override void RemoveEvent()
    {
        GameEventManager.MainInstance.RemoveEvent<EffectEventArgs>(EventHash.DamageTaken, OnDamageTaken);
    }
}
