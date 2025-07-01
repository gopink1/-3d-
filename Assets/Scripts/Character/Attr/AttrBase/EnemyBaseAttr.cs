using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ɫ�����������ԣ����������
/// </summary>
[Serializable]
public class EnemyBaseAttr
{
    [SerializeField]private float maxHp;    //�������ֵ
    [SerializeField] private float def;      //������
    [SerializeField] private float atk;       //������
    [SerializeField] private float speed;    //�ƶ��ٶ�

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
