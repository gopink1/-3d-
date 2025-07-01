using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBuilderParamParseStrategy : IJsonParseStrategy<Dictionary<string, WeaponBuilderAttr>>
{
    public override Dictionary<string, WeaponBuilderAttr> Parse(string fullPath)
    {
        Dictionary<string, WeaponBuilderAttr> weaponBuilderAttrs = new Dictionary<string, WeaponBuilderAttr>();

        // �ӵ�ַ�ж�ȡ�� textasset
        TextAsset textAsset = Resources.Load<TextAsset>(fullPath);
        if (textAsset == null)
        {
            Debug.LogError("û�е�ַΪ" + fullPath + "�ĵ�ַ�� textAsset");
            return null;
        }

        // תΪ�ַ���
        string jsontext = textAsset.text;
        if (string.IsNullOrEmpty(jsontext))
        {
            Debug.LogErrorFormat("��ַ" + fullPath + "���ڵĵ�ַ�������ļ�û������");
            return null;
        }

        // ����תΪ json �ļ�
        JsonData jsonData = JsonMapper.ToObject(jsontext);

        // ���� JSON ����
        for (int i = 0; i < jsonData.Count; i++)
        {
            string weaponIndex = jsonData[i]["weaponIndex"].ToString();
            string modelPath = jsonData[i]["modelPath"].ToString();
            string comboName = jsonData[i]["comboName"].ToString();

            WeaponBuilderAttr weaponBuilderAttr = new WeaponBuilderAttr(weaponIndex, modelPath, comboName);
            weaponBuilderAttrs[weaponIndex] = weaponBuilderAttr;
        }

        return weaponBuilderAttrs;
    }

    protected override void InitPath()
    {
        rootPath = "Cfg/";
    }
}
