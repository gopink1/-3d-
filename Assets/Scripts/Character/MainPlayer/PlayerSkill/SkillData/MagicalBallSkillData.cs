using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ħ�����ļ�������
/// </summary>
public class MagicalBallSkillData :SkillData
{
    private int index;//����id
    public int Index
    {
        get =>  index; 
    }
    private float skillRange;//���ܷ�Χ
    public float SkillRange
    {
        get => skillRange;
    }
    private float skillSpeed;//�����ٶ�
    public float SkillSpeed
    {
        get => SkillSpeed;
    }
    private float damage;
    public float Damage
    {
        get => damage;
    }
    public MagicalBallSkillData(int index,float skillRange,float skillSpeed,float damage, 
        string name, InputType inputType, TriggerType triggerType, string comboSOPath, string iconUIPath,string effectPrePath, string skillDescribe, float cool) : 
        base(index, name, inputType, triggerType, comboSOPath, iconUIPath,effectPrePath,skillDescribe, cool)
    {
        this.index = index;
        this.skillRange = skillRange;
        this.skillSpeed = skillSpeed;
        this.damage = damage;
    }
}
