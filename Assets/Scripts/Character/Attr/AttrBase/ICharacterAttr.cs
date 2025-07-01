using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICharacterAttr
{
    #region 基础属性
    private EnemyBaseAttr m_baseAttr;
    private float MaxHp;                       //生命值

    private float Def;                      //防御力
    private float attack;                   //攻击力
    private float moveSpeed;                 //移动速度

    private float currentHp;                //当前生命值

    private int Lv;                         //等级

    public float CurHP
    {
        get { return currentHp; }
        set { currentHp = value; }
    }

    public int CurLV
    {
        get { return Lv; }
        set { Lv = value; }
    }
    public float Speed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
    public float ATK
    {
        get { return attack; }
        set { attack = value; }
    }
    public float DEF
    {
        get { return Def; }
        set { Def = value; }
    }
    public float MaxHP
    {
        get { return MaxHP; }
        set { MaxHP = value; }
    }
    public float MaxMP
    {
        get { return MaxMP; }
        set { MaxMP = value; }
    }
    #endregion


    public ICharacterAttr(EnemyBaseAttr baseAttr,float hp)
    {

    }


}
