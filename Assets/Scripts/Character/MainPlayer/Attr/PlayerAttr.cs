using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class PlayerAttr
{
    [SerializeField] private float maxHp;    //最大生命值
    [SerializeField] private float maxMp;    //最大蓝量
    [SerializeField] private float def;      //防御力
    [SerializeField] private float atk;       //攻击力
    [SerializeField] private float speed;    //移动速度

    [SerializeField] private float currentHp;
    [SerializeField] private float currentMp;
    [SerializeField] private int Lv;

    public PlayerAttr(float maxH, float maxM, float def1, float atk1, float speed1, float currentHp1, float currentMp1, int lv1)
    {
        MaxHp=maxH;
        MaxMp=maxM;
        Def=def1;
        atk=atk1;
        Speed=speed1;
        this.currentHp=currentHp1;
        this.currentMp=currentMp1;
        Lv=lv1;
    }

    public float CurrentHp { get => currentHp; set => currentHp = Mathf.Clamp(value, 0f, maxHp); }
    public float CurrentMp { get => currentMp; set => currentMp = Mathf.Clamp(value, 0f, maxMp); }
    public int LV { get => Lv; set => Lv = value; }
    public float MaxHp { get => maxHp; set => maxHp=value; }
    public float MaxMp { get => maxMp; set => maxMp=value; }
    public float Def { get => def; set => def=value; }
    public float Atk { get => atk; set => atk=value; }
    public float Speed { get => speed; set => speed=value; }
}
