using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingSkillControl : MonoBehaviour
{
    private Transform m_Target;
    private float aliveTime;
    private float timer;
    float count;
    PlayerAttribute playerAttribute;
    public void InitSkill(Transform transform,float aliveTime,float count,PlayerAttribute playerAttribute)
    {
        m_Target = transform;
        this.aliveTime = aliveTime;
        this.count = count;
        this.playerAttribute = playerAttribute;
        OnBuffEffect();
    }

    private void OnBuffEffect()
    {
        PlayerHealthyControl scpr = m_Target.GetComponent<PlayerHealthyControl>();
        //BuffÉúÐ§
        switch (playerAttribute)
        {
            case PlayerAttribute.MaxHP:
                scpr.ModifyMaxHP(count,false);
                break;
            case PlayerAttribute.MaxMP:
                scpr.ModifyMaxMP(count, false);
                break;
            case PlayerAttribute.ATK:
                scpr.ModifyAtk(count, false);
                break;
            case PlayerAttribute.DEF:
                scpr.ModifyDef(count, false);
                break;
            case PlayerAttribute.SPEED:
                scpr.ModifySpeed(count, false);
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        transform.position = m_Target.position;
        if (timer > aliveTime)
        {
            timer = 0;
            OnSkillEnd();
            Destroy(gameObject);
        }
    }

    private void OnSkillEnd()
    {
        
    }
}
