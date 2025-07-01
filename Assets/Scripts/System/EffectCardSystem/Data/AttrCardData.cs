using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttrCardData : CardData
{
    //���Ըı俨�Ƶ�����
    //���Ըı���
    //atk,def,speed,maxHp,---

    private PlayerAttribute attributeType;  //��Ҫ�ı���ֵ������
    public PlayerAttribute AttributeType
    {
        get { return attributeType; }
    }
    private float count;                    //��ֵ
    public float Count
    {
        get { return count; }
    }
    private bool isPercentage;              //�Ƿ�Ϊ�ٷֱȵ�����
    public bool IsPercentage
    {
        get { return isPercentage; }
    }

    public AttrCardData(int id, string cardUIPath, CardType cardType,PlayerAttribute attributeType, float count,bool isPercentage) : base(id,cardUIPath,cardType)
    {
        this.attributeType = attributeType;
        this.count = count;
        this.isPercentage = isPercentage;
    }
}
