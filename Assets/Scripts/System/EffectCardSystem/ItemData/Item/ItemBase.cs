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
        //初始化子类实现
        itemData = data;
    }

    public void RegistEvent()
    {
        //捡起来所有的物品
        foreach (var effect in itemEffects)
        {
            effect.RegistEvent();
        }
    }

    public void RemoveEvent()
    {
        //捡起来所有的物品
        foreach (var effect in itemEffects)
        {
            effect.RemoveEvent();
        }
    }
}
