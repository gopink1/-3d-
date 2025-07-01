using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色基础共享属性，不变的属性
/// </summary>
[Serializable]
public class EnemyBaseAttr
{
    [SerializeField]private float maxHp;    //最大生命值
    [SerializeField] private float def;      //防御力
    [SerializeField] private float atk;       //攻击力
    [SerializeField] private float speed;    //移动速度

    [SerializeField] private string name;

    public EnemyBaseAttr(float maxHp, float def, float atk, float speed, string name)
    {
        this.maxHp=maxHp;
        this.def=def;
        this.atk=atk;
        this.speed=speed;
        this.name=name;
    }

    public float GetMaxHP()
    {
        return maxHp;
    }
    public float GetDef()
    {
        return def;
    }
    public float GetAtk()
    {
        return atk;
    }

    public float GetSpeed()
    {
        return speed;
    }
}
