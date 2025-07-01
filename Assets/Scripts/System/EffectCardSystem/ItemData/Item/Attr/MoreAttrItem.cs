using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreAttrItem : ItemBase
{
    public MoreAttrItem(ItemDataBase data)
    {
        InitItem(data);
    }
    public override void InitItem(ItemDataBase data)
    {
        base.InitItem(data);
        AttrItemDataD data1 = (AttrItemDataD)data;
        for (int i = 0; i < data1.AttrType.Length; i++)
        {
            itemEffects.Add(new PickToUpAttrEffect(data1.AttrType[i], data1.Value[i]));
        }
    }
}
