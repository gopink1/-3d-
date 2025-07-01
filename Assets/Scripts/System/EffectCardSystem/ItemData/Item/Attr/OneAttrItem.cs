using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneAttrItem : ItemBase
{
    public OneAttrItem(ItemDataBase data)
    {
        InitItem(data);
    }
    public override void InitItem(ItemDataBase data)
    {
        base.InitItem(data);
        AttrItemData data1 = (AttrItemData)data;
        itemEffects.Add(new PickToUpAttrEffect(data1.AttrType, data1.Value));
    }
}
