using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 收到伤害的效果
/// </summary>
public interface IDamageTakenEffect
{
    public void OnDamageTaken(EffectEventArgs args);
}
