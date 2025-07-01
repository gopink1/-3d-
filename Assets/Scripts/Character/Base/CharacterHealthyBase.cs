using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterHealthyBase : MonoBehaviour
{

    protected Animator m_Animator;

    protected virtual void Awake()
    {
        m_Animator = gameObject.GetComponent<Animator>();
    }
    public virtual void HitAction(float damage, string hitName, Transform atker, Transform self) 
    {

    }
    protected virtual void HealingAction(float healingCount, string healingName, Transform self)
    {

    }
    public void InitAddEvent()
    {
        GameEventManager.MainInstance.AddEventListening<float, string, Transform, Transform>(EventHash.OnCharacterHit, HitAction);
        GameEventManager.MainInstance.AddEventListening<float, string, Transform>(EventHash.OnCharacterHealing, HealingAction);
    }

    public void RemoveEvent()
    {
        GameEventManager.MainInstance.RemoveEvent<float, string, Transform, Transform>(EventHash.OnCharacterHit, HitAction);
        GameEventManager.MainInstance.RemoveEvent<float, string, Transform>(EventHash.OnCharacterHealing, HealingAction);
    }
}
