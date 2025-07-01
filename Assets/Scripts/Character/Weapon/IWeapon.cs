using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器基类
/// </summary>
public abstract class IWeapon
{
    //武器模型
    protected GameObject m_GameObject = null;   //武器模型
    protected GameObject m_Owner = null;        //武器拥有者

    //武器属性加成
    protected WeaponAttr m_Attr;

    public WeaponAttr Attr
    {
        get => m_Attr;
    }
    //武器的动作表
    protected CharacterComboSO m_ComboSO;

    public CharacterComboSO GetWeaponAtkSO()
    {
        return m_ComboSO;
    }
    public GameObject GetModel()
    {
        return m_GameObject;
    }
    public string GetAnimatorName()
    {
        return m_Attr.GetAnimatorName();
    }
    public void SetModel(GameObject model)
    {
        m_GameObject = model;
    }
    public void SetAttr(WeaponAttr attr)
    {
        m_Attr = attr;
    }
    public void SetComboSO(CharacterComboSO comboSO)
    {
        m_ComboSO = comboSO;
    }
    //武器的特效

    //武器的音效

    //武器攻击
    public void Attack() 
    {
        //动作
        //SO表传入外部外部处理动作相关逻辑
        //内部处理武器本身相关逻辑

        //特效
        ShowEffet();
        //音效
        ShowVoice();
    }
    protected abstract void ShowEffet();

    protected abstract void ShowVoice();

}
