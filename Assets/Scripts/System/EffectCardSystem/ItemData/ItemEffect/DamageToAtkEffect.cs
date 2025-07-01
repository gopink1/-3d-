using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageToAtkEffect : ItemEffectBase, IDamageTakenEffect
{
    public void OnDamageTaken(EffectEventArgs args)
    {
        //ÿ���յ��˺��ͻ����ӹ�����
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
