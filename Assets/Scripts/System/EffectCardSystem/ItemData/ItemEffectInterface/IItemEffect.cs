using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 饰品效果基类
/// </summary>
public interface IItemEffect
{
    //注册事件系统
    void RegistEvent();
    void RemoveEvent();
}
