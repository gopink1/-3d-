using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 改变属性的是触发
/// </summary>
public interface IChangeAttrEffect
{
    void ChangeAttr(EffectEventArgs args);
}
