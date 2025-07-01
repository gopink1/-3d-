using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������
/// </summary>
public abstract class IWeapon
{
    //����ģ��
    protected GameObject m_GameObject = null;   //����ģ��
    protected GameObject m_Owner = null;        //����ӵ����

    //�������Լӳ�
    protected WeaponAttr m_Attr;

    public WeaponAttr Attr
    {
        get => m_Attr;
    }
    //�����Ķ�����
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
    //��������Ч

    //��������Ч

    //��������
    public void Attack() 
    {
        //����
        //SO�����ⲿ�ⲿ����������߼�
        //�ڲ�����������������߼�

        //��Ч
        ShowEffet();
        //��Ч
        ShowVoice();
    }
    protected abstract void ShowEffet();

    protected abstract void ShowVoice();

}
