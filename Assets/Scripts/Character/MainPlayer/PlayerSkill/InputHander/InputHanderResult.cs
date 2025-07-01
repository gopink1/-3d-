using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 输入结果责任链的返回值
/// </summary>
public class InputHanderResult
{
    //普通触发需要参数
    private SkillData _skillData;//技能数据
    public SkillData SkillData
    {
        get { return _skillData; }
    }
    private int SkillInventoryIndex;
    public int InventoryIndex
    {
        get => SkillInventoryIndex;
    }

    //蓄力时候需要的数据
    private float chargeTime;//蓄力时间
    private bool isCharging;//是否正在蓄力
    public bool IsCharged
    {
        get => isCharging;
        set => isCharging = value;
    }
    public float ChargeTime 
    {
        get => chargeTime;
        set => chargeTime = value;
    }

    public InputHanderResult(SkillData skillData,int Skillindex)
    {
        _skillData=skillData;
        SkillInventoryIndex = Skillindex;
    }
    public InputHanderResult(SkillData skillData, float chargeTime, bool isCharging)
    {
        _skillData=skillData;
        ChargeTime=chargeTime;
        this.isCharging=isCharging;
    }

}
