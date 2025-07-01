using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalCircleSkillData : SkillData
{
    private int index;
    public int Index
    {
        get => index;
    }
    private Vector3 targetPos;
    public Vector3 TargetPos
    {
        get => targetPos;
    }
    private float circleRange;
    public float CircleRange
    {
        get => circleRange;
    }
    private float duration;
    public float Duration
    {
        get => duration;
    }
    private string effect;
    public string Effect
    {
        get => effect;
    }
    private float effectCount;
    public float EffectCount
    {
        get => effectCount;
    }
    private float effectRate;
    public float EffectRate
    {
        get => effectRate;
    }
    public MagicalCircleSkillData(int index, Vector3 targetPos, float circleRange, float duration,string effect,float effectCount, float effectRate,
        string name, InputType inputType, TriggerType triggerType, string comboSOPath, string iconUIPath,string effectPrePath, string skillDescribe, float cool) : 
        base(index, name, inputType, triggerType, comboSOPath,iconUIPath, effectPrePath,skillDescribe, cool)
    {
        {
            this.index=index;
            this.targetPos=targetPos;
            this.circleRange=circleRange;
            this.duration=duration;
            this.effect=effect;
            this.effectCount=effectCount;
            this.effectRate=effectRate;
        }
    }
}
