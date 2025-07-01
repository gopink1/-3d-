using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonParseFactory
{
    public  JsonParseContext<T> CreateParser<T>() where T : class
    {
        if (typeof(T) == typeof(Dictionary<int, EnemyBuildParm>))
        {
            //敌人生成的参数
            return new JsonParseContext<T>((new CharacterBuilderParseStrategy()) as IJsonParseStrategy<T>);
        }
        else if (typeof(T) == typeof(Dictionary<int, EnemyFSMParm>))
        {
            //敌人生成的状态机参数
            return new JsonParseContext<T>((new CharacterFsmParamParseStrategy()) as IJsonParseStrategy<T>);
        }
        else if (typeof(T) == typeof(Dictionary<string, WeaponBuilderAttr>))
        {
            //武器生成的参数
            return new JsonParseContext<T>((new WeaponBuilderParamParseStrategy()) as IJsonParseStrategy<T>);
        }
        else if (typeof(T) == typeof(Dictionary<string, WeaponAttr>))
        {
            //武器的基础属性
            return new JsonParseContext<T>((new WeaponAttrParseStrategy()) as IJsonParseStrategy<T>);
        }
        else if (typeof(T) == typeof(Dictionary<int, EnemyBaseAttr>))
        {
            //武器的基础属性
            return new JsonParseContext<T>((new EnemyBaseAttrParseStrategy()) as IJsonParseStrategy<T>);
        }
        else if (typeof(T) == typeof(Dictionary<string, ComboData>))
        {
            //武器的基础属性
            return new JsonParseContext<T>((new EnemyComboParseStrategy()) as IJsonParseStrategy<T>);
        }

        throw new ArgumentException($"不支持的类型: {typeof(T).FullName}");
    }
}
