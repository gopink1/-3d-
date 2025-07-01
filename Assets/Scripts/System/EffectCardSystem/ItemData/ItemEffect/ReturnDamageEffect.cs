using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnDamageEffect : ItemEffectBase, IDamageTakenEffect
{

    OneValueChangeItemData data;
    public ReturnDamageEffect(OneValueChangeItemData data)
    {
        this.data = data;
    }
    public void OnDamageTaken(EffectEventArgs args)
    {
        //当收到伤害就会反弹给敌人10%
        DamagetakenArgs ar = args as DamagetakenArgs;
        Transform atker = ar.Attacker.transform;
        float damage = ar.Damage;

        //根据伤害量返还敌人
        if (atker.TryGetComponent<CharacterHealthyBase>(out CharacterHealthyBase baseComponent))
        {
            // 成功获取基类组件
            baseComponent.HitAction(damage * data.Value, null,GameBase.MainInstance.GetMainPlayer().transform,atker);// 调用基类方法
        }
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
