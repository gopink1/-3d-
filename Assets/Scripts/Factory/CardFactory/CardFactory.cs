using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFactory
{
    private Dictionary<int, SkillData> skillDataMap;//�ܱ�
    //Dictionary<int, SkillCardData> skillCards = new Dictionary<int, SkillCardData>();
    private Dictionary<int, ItemDataBase> itemDataMap;
    private Dictionary<int, ItemBase> itemMap;


    public CardFactory() 
    {
        InitSkillData();
        InitItemData();
        //��������
        //�Ӽ���ϵͳ�л�ȡ�����ܵ�id


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
        MagicalCircleSkillData s4 = new MagicalCircleSkillData(4, new Vector3(0, 0, 4f), 5f, 4f, "damage+20f", 40f, 1f, "RedEnergyCircle", InputType.Instant, TriggerType.MagicalCircle,
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
    private void InitItemData()
    {
        itemDataMap = new Dictionary<int, ItemDataBase>();
        itemMap = new Dictionary<int, ItemBase>();
        AttrItemData id0 = new AttrItemData(0, "RedCrystal", "Icon/ItemIcon/RedCrystal", ItemType.AttrUp, "��ˮ����ȡ��+100����ֵ����", PlayerAttribute.MaxHP, 100f, false);
        OneAttrItem i0 = new OneAttrItem(id0);

        AttrItemDataD id1 = new AttrItemDataD(0, "Belt", "Icon/ItemIcon/Belt", ItemType.AttrUp, "Ƥ��������ȡ������20%����ֵ��10%�ٶ�",
            new PlayerAttribute[] { PlayerAttribute.MaxHP, PlayerAttribute.SPEED },
            new float[]{0.2f,0.1f}, 
            new bool[] { true, true });
        MoreAttrItem i1 = new MoreAttrItem(id1);

        AttrItemData id2 = new AttrItemData(0, "IronArmor", "Icon/ItemIcon/Iron Armor", ItemType.AttrUp, "�������ף���ȡ��+10������", PlayerAttribute.DEF, 10f, false);
        OneAttrItem i2 = new OneAttrItem(id2);

        AttrItemData id3 = new AttrItemData(0, "LeatherBoot", "Icon/ItemIcon/Leather Boot", ItemType.AttrUp, "Ƥ��Ь�ӻ�ȡ��С�������ٶ�", PlayerAttribute.SPEED, 2f, true);
        OneAttrItem i3 = new OneAttrItem(id3);

        AttrItemData id4 = new AttrItemData(0, "Wizard Hat", "Icon/ItemIcon/Wizard Hat", ItemType.AttrUp, "��ʦñ���������ֵ100f", PlayerAttribute.MaxMP, 100f, false);
        OneAttrItem i4 = new OneAttrItem(id4);

        AttrItemData id5 = new AttrItemData(0, "Feather", "Icon/ItemIcon/Feather", ItemType.AttrUp, "������ë��С�����ӹ�����", PlayerAttribute.ATK, 0.2f, false);
        OneAttrItem i5 = new OneAttrItem(id5);


        OneValueChangeItemData id6 = new OneValueChangeItemData(0, "Genmancer", "Icon/ItemIcon/Geomancer", ItemType.AtkTrigger, "���ף�����10%�˺�",0.1f);
        TakenDToAtkTriggerItem i6 = new TakenDToAtkTriggerItem(id6);

        OneValueChangeItemData id7 = new OneValueChangeItemData(0, "Arcanist", "Icon/ItemIcon/Arcanist", ItemType.AttrUp, "а�ۣ�����˺�С��������", 9f);
        ArcanistItem i7 = new ArcanistItem(id7);


        itemMap.Add(0, i0);
        itemMap.Add(1, i1);
        itemMap.Add(2, i2);
        itemMap.Add(3, i3);
        itemMap.Add(4, i4);
        itemMap.Add(5, i5);
        itemMap.Add(6, i6);
        itemMap.Add(7, i7);

    }
    public CardBase CreateAttrCard()
    {
        //�������Կ�
        //1.��ȡԤ����
        GameObject obj = Resources.Load("UI/Panel/InstancePanel/AttrCard") as GameObject;
        //2.��ȡ�ű�
        GameObject ins =  GameObject.Instantiate(obj);
        ins.SetActive(false);
        AttrCard card = ins.GetComponent<AttrCard>();
        return card;
    }
    public CardBase CreateSkillCard(int skillid)
    {
        //�������ܿ�Ƭ������
        //��������Ԥ����UI
        GameObject obj = Resources.Load("UI/Panel/InstancePanel/SkillCard") as GameObject;
        GameObject ins = GameObject.Instantiate(obj);
        ins.SetActive(false);
        SkillCard card = ins.GetComponent<SkillCard>();

        //���ÿ�������
        if (!skillDataMap.ContainsKey(skillid))
        {
            Debug.LogWarning("����û��idΪ"+skillid+"�ļ���");
        }
        card.SetCardData(skillDataMap[skillid]);
        return card;
    }
    public CardBase CreateItemCard(int itemId)
    {
        GameObject obj = Resources.Load("UI/Panel/InstancePanel/ItemCard") as GameObject;
        GameObject ins = GameObject.Instantiate(obj);
        ins.SetActive(false);
        ItemCard card = ins.GetComponent<ItemCard>();

        //����id�½���Ʒ��

        //���ÿ�������
        if (!itemMap.ContainsKey(itemId))
        {
            Debug.LogWarning("����û��idΪ"+itemId+"�ļ���");
        }
        card.SetCardData(itemMap[itemId]);
        return card;
    }
}
