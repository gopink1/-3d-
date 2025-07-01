using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerAttribute
{
    MaxHP,
    MaxMP,
    ATK,
    DEF,
    SPEED
}

public class PlayerDataSystem : IGameSystem
{
    //角色系统
    //保存有角色的数据
    //角色当前武器
    //角色当前携带技能
    //角色的属性
    private PlayerHealthyControl healthyControl;
    private Dictionary<PlayerAttribute, Action<float, bool>> attributeModifiers;
    // 事件系统
    public event Action<PlayerAttribute> OnAttributeChanged;

    private PlayerCombatControl combatControl;

    private PlayerMovementControl movementControl;


    public PlayerDataSystem(GameBase gameBase) : base(gameBase)
    {
        Init();
    }

    public override void Init()
    {
        //初始化系统获取到玩家对象身上的脚本
        GameObject p =  m_GameBase.GetMainPlayer();
        healthyControl = p.GetComponent<PlayerHealthyControl>();
        combatControl = p.GetComponent<PlayerCombatControl>();
        movementControl = p.GetComponent<PlayerMovementControl>();

        //初始化属性修改的系统的委托绑定
        attributeModifiers = new Dictionary<PlayerAttribute, Action<float, bool>>();
        attributeModifiers.Add(PlayerAttribute.MaxHP, healthyControl.ModifyMaxHP);
        attributeModifiers.Add(PlayerAttribute.MaxMP, healthyControl.ModifyMaxMP);
        attributeModifiers.Add(PlayerAttribute.ATK, healthyControl.ModifyAtk);
        attributeModifiers.Add(PlayerAttribute.DEF, healthyControl.ModifyDef);
        attributeModifiers.Add(PlayerAttribute.SPEED, healthyControl.ModifySpeed);


        GameEventManager.MainInstance.AddEventListening<CardData>(EventHash.ADDAttrCard, AddCard);
        GameEventManager.MainInstance.AddEventListening<SkillBar, SkillCardData>(EventHash.EquipSkill, EquipSkill);
    }

    public override void Release()
    {
        GameEventManager.MainInstance.RemoveEvent<CardData>(EventHash.ADDAttrCard, AddCard);
        GameEventManager.MainInstance.RemoveEvent<SkillBar, SkillCardData>(EventHash.EquipSkill, EquipSkill);
        OnAttributeChanged = null;
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }

    #region 更新角色属性系统相关方法
    /// <summary>
    /// 传入一个需要改变属性的枚举然后根据枚举改变属性
    /// </summary>
    public void ModifyAttrBuite(PlayerAttribute attribute, float value, bool isPercentage = false)
    {
        attributeModifiers[attribute].Invoke(value, isPercentage);
        //更新UI
        PMFacade.MainInstance.SendNotification(PMConst.PAttrUpdateCommand, attribute);

        // 触发属性变更事件
        OnAttributeChanged?.Invoke(attribute);
    }

    public void AddCard(CardData data)
    {
        Debug.Log("选择的卡片的数据为" + data);
        if(data.CardType != CardType.AttrCard)
        {
            Debug.Log("错误不是属性卡");
            return;
        }
        AttrCardData d = (AttrCardData)data;
        Debug.Log("选择的卡片的数据为" + d.AttributeType);
        Debug.Log("选择的卡片的数据为" + d.Count);
        Debug.Log("选择的卡片的数据为" + d.IsPercentage);
        ModifyAttrBuite(d.AttributeType, d.Count, d.IsPercentage);
    }
    #endregion
    #region 更新角色技能系统相关方法
    public Dictionary<SkillBar,SkillData> GetEquipedSkill()
    {
        return combatControl.EqiupedSkil;
    }
    public void EquipSkill(SkillBar skillBar,SkillCardData data)
    {
        //装备skill
        combatControl.EquipSkillCard(skillBar, data.SkillData);
    }
    #endregion
    #region 更新角色已拥有饰品

    #endregion

}
