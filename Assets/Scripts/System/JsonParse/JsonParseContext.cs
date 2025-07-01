using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class JsonCfgName
{
    public static readonly string CharacterBuilderCfg = "CharacterBuilderParamCfg";
    public static readonly string CharacterFsmParamCfg = "CharacterFsmParamCfg";
    public static readonly string WeaponAttrCfg = "WeaponAttrCfg";
    public static readonly string WeaponBuilderParamCfg = "WeaponBuilderParamCfg";
    public static readonly string EnemyBaseAttrCfg = "EnemyBaseAttrCfg";
    public static readonly string EnemyComboCfg = "EnemyComboCfg";
}

public class JsonParseContext<T>
{
    //策略模式的客户端
    private IJsonParseStrategy<T> m_strategy;

    public JsonParseContext(IJsonParseStrategy<T> strategy)
    {
        this.m_strategy = strategy;
    }

    public T ParseJsonContext(string jsonName)
    {
        Debug.Log(m_strategy.GetRootPath() + jsonName + "!!!!!!!!!!!!!!!!!!!!!!!!!!");
        return m_strategy.Parse(m_strategy.GetRootPath() + jsonName);
    }

}
