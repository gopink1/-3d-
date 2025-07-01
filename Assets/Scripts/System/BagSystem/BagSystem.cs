using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BagSystem : IGameSystem
{
    //背包系统，存储玩家的背包数据
    //当前获取到的卡牌增益
    private List<AttrCardData> attrCards;
    //当前获取到的饰品
    private List<ItemCardData> itemCards;
    //当前获取到的技能卡牌
    private Dictionary<SkillBar,SkillCardData> skillCards;
    public Dictionary<SkillBar,SkillCardData> SkillCards { get { return skillCards; } }
    //当前的金币数量



    public BagSystem(GameBase gameBase) : base(gameBase)
    {
        Init();
    }

    public override void Init()
    {
        attrCards = new List<AttrCardData>();
        itemCards = new List<ItemCardData>();
        skillCards = new Dictionary<SkillBar, SkillCardData>();
        skillCards.Add(SkillBar.Q, null);
        skillCards.Add(SkillBar.E, null);
        GameEventManager.MainInstance.AddEventListening<CardData>(EventHash.ADDAttrCard, ADDCard);
        GameEventManager.MainInstance.AddEventListening<SkillBar, SkillCardData>(EventHash.EquipSkill, EquipSkill);
        GameEventManager.MainInstance.AddEventListening<ItemCardData>(EventHash.ADDItemCard, AddItemCard);
    }

    public override void Release()
    {
        GameEventManager.MainInstance.RemoveEvent<CardData>(EventHash.ADDAttrCard, ADDCard);
        GameEventManager.MainInstance.RemoveEvent<SkillBar, SkillCardData>(EventHash.EquipSkill, EquipSkill);
        GameEventManager.MainInstance.RemoveEvent<ItemCardData>(EventHash.ADDItemCard, AddItemCard);
        attrCards = null; itemCards = null; skillCards = null;
    }

    public override void Update()
    {
        
    }

    private void ADDCard(CardData data)
    {
        
        if (data == null)
        {
            Debug.Log("传入卡牌的数据为空错误！！！！！！！！！！！");
            return ;
        }
        //传入类型和data进行对卡牌的存储
        if(data.CardType != CardType.AttrCard)
        {
            Debug.Log("传入卡牌类型不是属性错误！！！！！！！！！！！");
        }
        attrCards.Add(data as AttrCardData);
    }
    private void EquipSkill(SkillBar skillbar, SkillCardData data)
    {

        if (data == null)
        {
            Debug.Log("传入卡牌的数据为空错误！！！！！！！！！！！");
            return;
        }
        //传入类型和data进行对卡牌的存储
        if(data.CardType != CardType.SkillCard)
        {
            Debug.Log("传入卡牌的不是技能卡错误！！！！！！！！！！！");
            return;
        }
        skillCards[skillbar] = data;
        Debug.Log("装备名字为"+ data.SkillData.Name);
    }

    public void AddItemCard(ItemCardData card)
    {
        //激活
        if(card == null)
        {
            return;
        }
        itemCards.Add(card);
        ActiveItem(card);
    }
    public void RemoveItemCard(ItemCardData card)
    {
        //激活
        if (card == null)
        {
            return;
        }
        itemCards.Remove(card);
        RemoveItem(card);
    }

    private void ActiveItem(ItemCardData data)
    {
        ItemBase item = data.Item;
        item.RegistEvent();
    }
    private void RemoveItem(ItemCardData data)
    {
        ItemBase item = data.Item;
        item.RemoveEvent();
    }
}
