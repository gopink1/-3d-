using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcanistItem : ItemBase
{
    public ArcanistItem(ItemDataBase data)
    {
        InitItem(data);
    }
    public override void InitItem(ItemDataBase data)
    {
        base.InitItem(data);
        OneValueChangeItemData data1 = (OneValueChangeItemData)data;
        itemEffects.Add(new PDamageChangeEffect(data1.Value));
    }
}
