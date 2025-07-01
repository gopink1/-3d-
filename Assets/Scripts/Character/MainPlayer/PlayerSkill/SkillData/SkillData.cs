using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum InputType
{
    Instant,
    Charge,
    Aim
}
public enum TriggerType
{
    BuffOwn,
    MagicalCircle,
    MagicalBall,
    ChargeCombo
}

public class SkillData
{
    protected int id;//技能代号
    public int Id
    {
        get => id;
    }
    protected string name;//技能名字
    public string Name
    {
        get => name; 
    }
    protected InputType inputType;//输入类型（用于责任链选择输入方式）
    public InputType InputType
    {
        get => inputType;
    }
    protected TriggerType triggerType;//触发方式（用于选择策略模式进行对技能触发）
    public TriggerType TriggerType
    {
        get => triggerType;
    }
    protected string comboSOPath;//玩家的路径（保存有连招的动作，伤害等信息）
    public string ComboSOPath
    {
        get => comboSOPath;
    }
    protected string iconUIPath;//技能图标路径
    public string IconUIPath
    {
        get => iconUIPath;
    }
    protected string effectPrePath;//技能特效预制体
    public string EffectPrePath
    {
        get => effectPrePath;
    }

    protected string skillDescribe;
    public string SkillDescribe
    {
        get { return skillDescribe; }
    }
    protected float coolTime;
    public float CoolTime
    {
        get { return coolTime; }
        set { coolTime = value; }
    }

    public SkillData(int index, string name, InputType inputType, TriggerType triggerType, string comboSOPath,string iconUIPath, string effectPrePath,string skillDescribe, float coolTime)
    {
        this.id=index;
        this.name=name;
        this.inputType=inputType;
        this.triggerType=triggerType;
        this.comboSOPath=comboSOPath;
        this.iconUIPath=iconUIPath;
        this.effectPrePath=effectPrePath;
        this.skillDescribe=skillDescribe;
        this.coolTime=coolTime;
    }
}
