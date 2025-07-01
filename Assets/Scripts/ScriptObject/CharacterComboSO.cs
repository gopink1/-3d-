using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Combo", menuName = "Create/Character/Combo", order = 1)]
public class CharacterComboSO : ScriptableObject
{
    [SerializeField] private List<CharacterComboDataSO> allComboData = new List<CharacterComboDataSO>();

    //获取连招中的动作
    public string TryGetOneComboAction(int index)
    {
        if (allComboData.Count == 0) return null;
        return allComboData[index].ComboName;
    }
    //获取伤害动画第几个招式的第几段伤害的动画
    public string TryGetOneHitComboAction(int index, int hitIndex)
    {
        if (allComboData.Count == 0) return null;//连招表为空爆
        if (allComboData[index].GetHitAndParryMaxCount() == 0) return null;//没有配置爆空引用
        //Debug.Log(index +"+" + hitIndex);
        return allComboData[index].ComboHitName[hitIndex];

    }
    //获取格挡动画第几个招式的第几段伤害的动画
    public string TryGetOneParryComboAction(int index, int ParryIndex)
    {
        if (allComboData.Count == 0) return null;//连招表为空爆
        if (allComboData[index].GetHitAndParryMaxCount() == 0) return null;//没有配置爆空引用
        return allComboData[index].ComboParryName[ParryIndex];
    }
    /// <summary>
    /// 伤害值
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float TryGetDmage(int index)
    {
        if (allComboData.Count == 0) return 0f;
        return allComboData[index].Damage;
    }
    /// <summary>
    /// 冷却时间
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float TryGetColdTime(int index)
    {
        if (allComboData.Count == 0) return 0f;
        return allComboData[index].ColdTime;
    }
    public Vector2 TryGetComboMatchTime(int index)
    {
        if (allComboData.Count == 0) return Vector2.zero;
        return allComboData[index].MatchTime;
    }

    /// <summary>
    /// 最佳距离
    /// 多用于处决等动画播放进行匹配
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float TryGetComboPositionOffset(int index)
    {
        if (allComboData.Count == 0) return 0f;
        return allComboData[index].ComboPositonOffset;
    }
    /// <summary>
    /// 获取攻击范围
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float TryGetComboAtkRange(int index)
    {
        if (allComboData.Count == 0) return 0f;
        return allComboData[index].ComboAtkRange;
    }
    /// <summary>
    /// 获取攻击角度
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float TryGetComboAngleRange(int index)
    {
        if(allComboData.Count == 0)return 0f;
        return allComboData[index].ComboAngleRange;
    }
    public int TryGetHitAndParryMaxCount(int index) => allComboData[index].GetHitAndParryMaxCount();//连招中某一段的打击次数（伤害段数量）

    public int TryGetComboMaxCount() => allComboData.Count;//连招次数

}

