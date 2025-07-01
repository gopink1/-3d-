using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAttrArgs : EffectEventArgs
{
    private PlayerAttribute attributeType;//改变的属性类型
    public PlayerAttribute ArrtibuteType
    {
        get { return attributeType; }
    }
    private float value;//改变的量
    public float Value
    {
        get { return value; }
    }
    private bool isPersentage;//改变的量
    public float IsPersentage
    {
        get { return value; }
    }
    public ChangeAttrArgs(PlayerAttribute attributeType, float value,bool isPersentage)
    {
        this.attributeType=attributeType;
        this.value=value;
        this.isPersentage=isPersentage;
    }
}
