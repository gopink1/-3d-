using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAttrArgs : EffectEventArgs
{
    private PlayerAttribute attributeType;//�ı����������
    public PlayerAttribute ArrtibuteType
    {
        get { return attributeType; }
    }
    private float value;//�ı����
    public float Value
    {
        get { return value; }
    }
    private bool isPersentage;//�ı����
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
