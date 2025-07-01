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
    protected int id;//���ܴ���
    public int Id
    {
        get => id;
    }
    protected string name;//��������
    public string Name
    {
        get => name; 
    }
    protected InputType inputType;//�������ͣ�����������ѡ�����뷽ʽ��
    public InputType InputType
    {
        get => inputType;
    }
    protected TriggerType triggerType;//������ʽ������ѡ�����ģʽ���жԼ��ܴ�����
    public TriggerType TriggerType
    {
        get => triggerType;
    }
    protected string comboSOPath;//��ҵ�·�������������еĶ������˺�����Ϣ��
    public string ComboSOPath
    {
        get => comboSOPath;
    }
    protected string iconUIPath;//����ͼ��·��
    public string IconUIPath
    {
        get => iconUIPath;
    }
    protected string effectPrePath;//������ЧԤ����
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
