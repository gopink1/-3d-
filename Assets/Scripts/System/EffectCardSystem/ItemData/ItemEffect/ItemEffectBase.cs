using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// 物品效果基类
/// </summary>
public abstract class ItemEffectBase : IItemEffect
{
    public abstract void RegistEvent();

    public abstract void RemoveEvent();
}
