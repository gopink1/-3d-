using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttrCardData : CardData
{
    //属性改变卡牌的数据
    //属性改变有
    //atk,def,speed,maxHp,---

    private PlayerAttribute attributeType;  //需要改变数值的类型
    public PlayerAttribute AttributeType
    {
        get { return attributeType; }
    }
    private float count;                    //数值
    public float Count
    {
        get { return count; }
    }
    private bool isPercentage;              //是否为百分比的增量
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
