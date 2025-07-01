using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��ҵļ���ϵͳ��
/// ����洢���еļ�������
/// ���԰���������ļ��м��ؽ������ݵ��ڴ���
/// </summary>
public class PlayerSkillSystem : IGameSystem
{
    private Dictionary<int,SkillData> skillDataMap;//�ܱ�
    public PlayerSkillSystem(GameBase gameBase) : base(gameBase)
    {
        Init();
    }

    public override void Init()
    {
        InitSkillData();
    }

    private void InitSkillData()
    {
        skillDataMap = new Dictionary<int, SkillData>();
        MagicalBallSkillData s0 = new MagicalBallSkillData(0, 20f, 10f, 20f, "fireball", InputType.Instant, TriggerType.MagicalBall,
        "ComboSO/Player/FireCombo/FireBallCombo0",
        "Icon/IceFire",
        "SkillPrefab/IceFire",
        "��������������˻ᱬը",
        10f);
        MagicalCircleSkillData s1 = new MagicalCircleSkillData(1, Vector3.zero, 5f, 4f, "hp+10f/s", 10f, 0.95f, "HealingCircle", InputType.Instant, TriggerType.MagicalCircle,
        "ComboSO/Player/HealingCircle/HealingCircleCombo0",
        "Icon/HealingCircle",
        "SkillPrefab/HealingCircle",
        "�������Ʒ���",
        10f);

        BuffSkillData s2 = new BuffSkillData(2, 6f, PlayerAttribute.ATK, 10f, "upAtkBuff", InputType.Instant, TriggerType.BuffOwn,
        "ComboSO/Player/UpOwnCombo/UpOwnAttrCombo0",
        "Icon/Buff",
        "SkillPrefab/Buff",
        "�ӹ�������buff���������10�㹥����",
        15f);

        ChargeComboSkillData s3 = new ChargeComboSkillData(3, 2.5f, new Vector3(0, 0.9f, 1.5f), "ChargeHeavyHoriAtk", InputType.Charge, TriggerType.ChargeCombo,
        "ComboSO/Player/ChargeHeavyHoriAtk/ChargeHeavyHoriAtkCombo0",
        "Icon/Electro slash",
        "SkillPrefab/Electro slash",
        "������������ʱ��Խ��Ч������Խ��",
        5f);
        MagicalCircleSkillData s4 = new MagicalCircleSkillData(4, new Vector3(0, 0, 4f), 5f, 4f, "damage+20f", 20f, 0f, "RedEnergyCircle", InputType.Instant, TriggerType.MagicalCircle,
        "ComboSO/Player/RedEnergyCircle/RedEnergyCircleCombo0",
        "Icon/RedEnergyCircle",
        "SkillPrefab/RedEnergyCircle",
        "������ɫѪʯ����",
        5f);

        skillDataMap.Add(0, s0);
        skillDataMap.Add(1, s1);
        skillDataMap.Add(2, s2);
        skillDataMap.Add(3, s3);
        skillDataMap.Add(4, s4);
    }

    public override void Release()
    {
        skillDataMap.Clear();
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
    // ����ID��ȡ����
    public SkillData GetSkillById(int id)
    {
        if (skillDataMap.TryGetValue(id, out SkillData skill))
        {
            return skill;
        }

        Debug.LogWarning($"δ�ҵ�IDΪ {id} �ļ���");
        return null;
    }
}
