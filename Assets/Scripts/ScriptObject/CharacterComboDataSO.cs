using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboDate", menuName = "Create/Character/ComboData", order = 0)]
public class CharacterComboDataSO : ScriptableObject
{
    //招式名称
    [SerializeField] private string comboName;//招式的名字
    [SerializeField] private string[] comboParryName;//对应的格挡动画s
    [SerializeField] private string[] comboHitName;//当前招式打击的次数（单动画造成多次伤害）
    [SerializeField] private float damage;//造成的伤害
    [SerializeField] private float coldTime;//连招的冷却事件衔接下一个动作的时间
    [SerializeField] private Vector2 matchTime;//动画匹配的时间段% 
    [SerializeField] private float comboPositionOffset;//两者的最佳距离
    [SerializeField] private float comboAtkRange;//攻击范围
    [SerializeField] private float comboAngleRange;//攻击角度范围
    public string ComboName => comboName;
    public string[] ComboParryName => comboParryName;
    public string[] ComboHitName => comboHitName;
    public float ColdTime => coldTime;
    public Vector2 MatchTime => matchTime;
    public float Damage => damage;
    public float ComboPositonOffset => comboPositionOffset;
    public float ComboAtkRange => comboAtkRange;
    public float ComboAngleRange => comboAngleRange;


    //获取当前动作的最大受伤数量
    public int GetHitAndParryMaxCount() => comboHitName.Length;

}
