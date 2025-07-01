using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeComboSkillData : SkillData
{
    private int index;
    public int Index
    {
        get => index; 
    }

    private float maxChargeTime;
    public float MaxChargeTime
    {
        get => maxChargeTime;
    }

    private Vector3 relativeOffset;
    public Vector3 RelativeOffset
    {
        get => relativeOffset;
    }

    public ChargeComboSkillData(int index, float maxChargeTime, Vector3 relativeOffset,
        string name, InputType inputType, TriggerType triggerType, string comboSOPath, string iconUIPath, string effectPrePath, string skillDescribe, float cool) : 
        base(index, name, inputType, triggerType, comboSOPath, iconUIPath,effectPrePath,skillDescribe, cool)
    {
        this.index = index;
        this.maxChargeTime = maxChargeTime;
        this.relativeOffset = relativeOffset;
    }
}
