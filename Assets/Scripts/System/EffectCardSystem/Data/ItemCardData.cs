using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCardData : CardData
{
    //饰品的卡片数据
    //物品类
    private ItemBase item;//当前卡片关联物品
    public ItemBase Item
    {
        get { return item; }
    }
    public ItemCardData(int id, string cardUIPath, CardType cardType, ItemBase item) : base(id, cardUIPath, cardType)
    {
        this.item=item;
    }
}
