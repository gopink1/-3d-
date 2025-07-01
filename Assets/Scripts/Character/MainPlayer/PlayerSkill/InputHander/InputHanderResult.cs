using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �������������ķ���ֵ
/// </summary>
public class InputHanderResult
{
    //��ͨ������Ҫ����
    private SkillData _skillData;//��������
    public SkillData SkillData
    {
        get { return _skillData; }
    }
    private int SkillInventoryIndex;
    public int InventoryIndex
    {
        get => SkillInventoryIndex;
    }

    //����ʱ����Ҫ������
    private float chargeTime;//����ʱ��
    private bool isCharging;//�Ƿ���������
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
