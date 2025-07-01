using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class EnemyAttr
{
    [SerializeField] private EnemyBaseAttr baseAttr;

    [SerializeField] private float currentHp;


    public void SetAttr(EnemyBaseAttr a)
    {
        baseAttr = a;
        currentHp = baseAttr.GetMaxHP();
    }

    public float GetCurrentHp()
    {
        return currentHp;
    }

    public float MaxHP
    {
        get => baseAttr.GetMaxHP();
    }
    public float Def
    {
        get => baseAttr.GetDef();
    }
    public float Atk
    {
        get => baseAttr.GetAtk();
    }

    public float Speed
    {
        get => baseAttr.GetSpeed();
    }
    /// <summary>
    /// ¼õÉÙÉúÃüÖµ
    /// </summary>
    public void ReduceHP(float count)
    {
        currentHp -= count;
    }
}
