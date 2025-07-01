using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCardData : CardData
{
    //��Ʒ�Ŀ�Ƭ����
    //��Ʒ��
    private ItemBase item;//��ǰ��Ƭ������Ʒ
    public ItemBase Item
    {
        get { return item; }
    }
    public ItemCardData(int id, string cardUIPath, CardType cardType, ItemBase item) : base(id, cardUIPath, cardType)
    {
        this.item=item;
    }
}
