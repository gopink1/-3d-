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
    //��ɫϵͳ
    //�����н�ɫ������
    //��ɫ��ǰ����
    //��ɫ��ǰЯ������
    //��ɫ������
    private PlayerHealthyControl healthyControl;
    private Dictionary<PlayerAttribute, Action<float, bool>> attributeModifiers;
    // �¼�ϵͳ
    public event Action<PlayerAttribute> OnAttributeChanged;

    private PlayerCombatControl combatControl;

    private PlayerMovementControl movementControl;


    public PlayerDataSystem(GameBase gameBase) : base(gameBase)
    {
        Init();
    }

    public override void Init()
    {
        //��ʼ��ϵͳ��ȡ����Ҷ������ϵĽű�
        GameObject p =  m_GameBase.GetMainPlayer();
        healthyControl = p.GetComponent<PlayerHealthyControl>();
        combatControl = p.GetComponent<PlayerCombatControl>();
        movementControl = p.GetComponent<PlayerMovementControl>();

        //��ʼ�������޸ĵ�ϵͳ��ί�а�
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

    #region ���½�ɫ����ϵͳ��ط���
    /// <summary>
    /// ����һ����Ҫ�ı����Ե�ö��Ȼ�����ö�ٸı�����
    /// </summary>
    public void ModifyAttrBuite(PlayerAttribute attribute, float value, bool isPercentage = false)
    {
        attributeModifiers[attribute].Invoke(value, isPercentage);
        //����UI
        PMFacade.MainInstance.SendNotification(PMConst.PAttrUpdateCommand, attribute);

        // �������Ա���¼�
        OnAttributeChanged?.Invoke(attribute);
    }

    public void AddCard(CardData data)
    {
        Debug.Log("ѡ��Ŀ�Ƭ������Ϊ" + data);
        if(data.CardType != CardType.AttrCard)
        {
            Debug.Log("���������Կ�");
            return;
        }
        AttrCardData d = (AttrCardData)data;
        Debug.Log("ѡ��Ŀ�Ƭ������Ϊ" + d.AttributeType);
        Debug.Log("ѡ��Ŀ�Ƭ������Ϊ" + d.Count);
        Debug.Log("ѡ��Ŀ�Ƭ������Ϊ" + d.IsPercentage);
        ModifyAttrBuite(d.AttributeType, d.Count, d.IsPercentage);
    }
    #endregion
    #region ���½�ɫ����ϵͳ��ط���
    public Dictionary<SkillBar,SkillData> GetEquipedSkill()
    {
        return combatControl.EqiupedSkil;
    }
    public void EquipSkill(SkillBar skillBar,SkillCardData data)
    {
        //װ��skill
        combatControl.EquipSkillCard(skillBar, data.SkillData);
    }
    #endregion
    #region ���½�ɫ��ӵ����Ʒ

    #endregion

}
