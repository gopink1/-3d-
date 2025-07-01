using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家的技能系统，
/// 负责存储所有的技能数据
/// 可以按照需求从文件中加载节能数据到内存中
/// </summary>
public class PlayerSkillSystem : IGameSystem
{
    private Dictionary<int,SkillData> skillDataMap;//总表
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
        "发射火球碰到敌人会爆炸",
        10f);
        MagicalCircleSkillData s1 = new MagicalCircleSkillData(1, Vector3.zero, 5f, 4f, "hp+10f/s", 10f, 0.95f, "HealingCircle", InputType.Instant, TriggerType.MagicalCircle,
        "ComboSO/Player/HealingCircle/HealingCircleCombo0",
        "Icon/HealingCircle",
        "SkillPrefab/HealingCircle",
        "引导治疗法阵",
        10f);

        BuffSkillData s2 = new BuffSkillData(2, 6f, PlayerAttribute.ATK, 10f, "upAtkBuff", InputType.Instant, TriggerType.BuffOwn,
        "ComboSO/Player/UpOwnCombo/UpOwnAttrCombo0",
        "Icon/Buff",
        "SkillPrefab/Buff",
        "加攻击力的buff，开启后加10点攻击力",
        15f);

        ChargeComboSkillData s3 = new ChargeComboSkillData(3, 2.5f, new Vector3(0, 0.9f, 1.5f), "ChargeHeavyHoriAtk", InputType.Charge, TriggerType.ChargeCombo,
        "ComboSO/Player/ChargeHeavyHoriAtk/ChargeHeavyHoriAtkCombo0",
        "Icon/Electro slash",
        "SkillPrefab/Electro slash",
        "蓄力攻击蓄力时间越长效果次数越多",
        5f);
        MagicalCircleSkillData s4 = new MagicalCircleSkillData(4, new Vector3(0, 0, 4f), 5f, 4f, "damage+20f", 20f, 0f, "RedEnergyCircle", InputType.Instant, TriggerType.MagicalCircle,
        "ComboSO/Player/RedEnergyCircle/RedEnergyCircleCombo0",
        "Icon/RedEnergyCircle",
        "SkillPrefab/RedEnergyCircle",
        "引导红色血石砸下",
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
    // 根据ID获取技能
    public SkillData GetSkillById(int id)
    {
        if (skillDataMap.TryGetValue(id, out SkillData skill))
        {
            return skill;
        }

        Debug.LogWarning($"未找到ID为 {id} 的技能");
        return null;
    }
}
