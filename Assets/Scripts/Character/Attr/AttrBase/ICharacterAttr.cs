using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICharacterAttr
{
    #region ��������
    private EnemyBaseAttr m_baseAttr;
    private float MaxHp;                       //����ֵ

    private float Def;                      //������
    private float attack;                   //������
    private float moveSpeed;                 //�ƶ��ٶ�

    private float currentHp;                //��ǰ����ֵ

    private int Lv;                         //�ȼ�

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
