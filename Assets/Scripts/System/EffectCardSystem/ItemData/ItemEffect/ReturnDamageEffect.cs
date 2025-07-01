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
        //���յ��˺��ͻᷴ��������10%
        DamagetakenArgs ar = args as DamagetakenArgs;
        Transform atker = ar.Attacker.transform;
        float damage = ar.Damage;

        //�����˺�����������
        if (atker.TryGetComponent<CharacterHealthyBase>(out CharacterHealthyBase baseComponent))
        {
            // �ɹ���ȡ�������
            baseComponent.HitAction(damage * data.Value, null,GameBase.MainInstance.GetMainPlayer().transform,atker);// ���û��෽��
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
