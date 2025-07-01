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
            //�������ɵĲ���
            return new JsonParseContext<T>((new CharacterBuilderParseStrategy()) as IJsonParseStrategy<T>);
        }
        else if (typeof(T) == typeof(Dictionary<int, EnemyFSMParm>))
        {
            //�������ɵ�״̬������
            return new JsonParseContext<T>((new CharacterFsmParamParseStrategy()) as IJsonParseStrategy<T>);
        }
        else if (typeof(T) == typeof(Dictionary<string, WeaponBuilderAttr>))
        {
            //�������ɵĲ���
            return new JsonParseContext<T>((new WeaponBuilderParamParseStrategy()) as IJsonParseStrategy<T>);
        }
        else if (typeof(T) == typeof(Dictionary<string, WeaponAttr>))
        {
            //�����Ļ�������
            return new JsonParseContext<T>((new WeaponAttrParseStrategy()) as IJsonParseStrategy<T>);
        }
        else if (typeof(T) == typeof(Dictionary<int, EnemyBaseAttr>))
        {
            //�����Ļ�������
            return new JsonParseContext<T>((new EnemyBaseAttrParseStrategy()) as IJsonParseStrategy<T>);
        }
        else if (typeof(T) == typeof(Dictionary<string, ComboData>))
        {
            //�����Ļ�������
            return new JsonParseContext<T>((new EnemyComboParseStrategy()) as IJsonParseStrategy<T>);
        }

        throw new ArgumentException($"��֧�ֵ�����: {typeof(T).FullName}");
    }
}
