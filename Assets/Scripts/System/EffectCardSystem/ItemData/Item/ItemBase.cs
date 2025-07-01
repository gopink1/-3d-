using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase
{
    protected ItemDataBase itemData;
    public ItemDataBase ItemData
    {
        get { return itemData; }
    }
    protected List<ItemEffectBase> itemEffects = new List<ItemEffectBase>();

    public virtual void InitItem(ItemDataBase data)
    {
        //��ʼ������ʵ��
        itemData = data;
    }

    public void RegistEvent()
    {
        //���������е���Ʒ
        foreach (var effect in itemEffects)
        {
            effect.RegistEvent();
        }
    }

    public void RemoveEvent()
    {
        //���������е���Ʒ
        foreach (var effect in itemEffects)
        {
            effect.RemoveEvent();
        }
    }
}
