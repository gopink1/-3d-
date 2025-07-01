using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// buff类技能需要的参数
/// </summary>
public class BuffSkillData : SkillData
{
    private int index;
    public int Index
    {
        get => index;
    }
    private float duration;
    public float Duration
    {
        get => duration;
    }
    private PlayerAttribute effect;
    public PlayerAttribute Effect
    {
        get => effect;
    }
    private float effectCount;
    public float EffectCount
    {
        get => effectCount;
    }

    public BuffSkillData(int index,
        float duration, PlayerAttribute effect, float effectCount,
        string name, InputType inputType, TriggerType triggerType, string comboSOPath, string iconUIPath, string effectPrePath, string skillDescribe, float coolTime) :
        base(index, name, inputType, triggerType, comboSOPath, iconUIPath, effectPrePath, skillDescribe, coolTime)
    {

        this.index=index;
        this.duration=duration;
        this.effect=effect;
        this.effectCount=effectCount;
    }


}
