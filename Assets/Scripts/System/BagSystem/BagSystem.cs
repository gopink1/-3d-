using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BagSystem : IGameSystem
{
    //����ϵͳ���洢��ҵı�������
    //��ǰ��ȡ���Ŀ�������
    private List<AttrCardData> attrCards;
    //��ǰ��ȡ������Ʒ
    private List<ItemCardData> itemCards;
    //��ǰ��ȡ���ļ��ܿ���
    private Dictionary<SkillBar,SkillCardData> skillCards;
    public Dictionary<SkillBar,SkillCardData> SkillCards { get { return skillCards; } }
    //��ǰ�Ľ������



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
            Debug.Log("���뿨�Ƶ�����Ϊ�մ��󣡣�������������������");
            return ;
        }
        //�������ͺ�data���жԿ��ƵĴ洢
        if(data.CardType != CardType.AttrCard)
        {
            Debug.Log("���뿨�����Ͳ������Դ��󣡣�������������������");
        }
        attrCards.Add(data as AttrCardData);
    }
    private void EquipSkill(SkillBar skillbar, SkillCardData data)
    {

        if (data == null)
        {
            Debug.Log("���뿨�Ƶ�����Ϊ�մ��󣡣�������������������");
            return;
        }
        //�������ͺ�data���жԿ��ƵĴ洢
        if(data.CardType != CardType.SkillCard)
        {
            Debug.Log("���뿨�ƵĲ��Ǽ��ܿ����󣡣�������������������");
            return;
        }
        skillCards[skillbar] = data;
        Debug.Log("װ������Ϊ"+ data.SkillData.Name);
    }

    public void AddItemCard(ItemCardData card)
    {
        //����
        if(card == null)
        {
            return;
        }
        itemCards.Add(card);
        ActiveItem(card);
    }
    public void RemoveItemCard(ItemCardData card)
    {
        //����
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
