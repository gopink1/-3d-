using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏工厂
/// </summary>
public static class GameBaseFactory
{
    private static bool isLoadFromRes = true;
    private static IAssetFactory m_assetFactory = null;
    private static IWeaponFactory m_weaponFactory = null;
    private static IAttrFactory m_attrFactory = null;
    private static ICharacterFactory m_characterFactory = null;
    private static JsonParseFactory m_jsonparseFactory = null;
    private static CardFactory m_CardFactory = null;
    public static IAssetFactory GetAssetFactory() 
    { 
        if(m_assetFactory == null)
        {
            if(isLoadFromRes)
            {
                m_assetFactory= new ResourcesAssetFactory();
            }
            else
            {
                //其他的资源加载工厂进行产出
            }
        }
        return m_assetFactory;
    }
    public static IWeaponFactory GetWeaponFactory()
    {
        if(m_weaponFactory == null)
        {
            m_weaponFactory = new WeaponFactory();
        }
        return m_weaponFactory;
    }
    public static IAttrFactory GetAttrFactory()
    {
        if ( m_attrFactory == null)
        {
            m_attrFactory = new AttrFactory();
        }
        return m_attrFactory;
    }
    public static ICharacterFactory GetCharacterFactory()
    {
        if (m_characterFactory == null)
        {
            m_characterFactory = new CharacterFactory();
        }
        return m_characterFactory;
    }

    public static JsonParseFactory GetJsonParseFactory()
    {
        if(m_jsonparseFactory == null)
        {
            m_jsonparseFactory= new JsonParseFactory();
        }
        return m_jsonparseFactory;
    }

    public static CardFactory GetCardFactory()
    {
        if (m_CardFactory == null)
        {
            m_CardFactory= new CardFactory();
        }
        return m_CardFactory;
    }

}
