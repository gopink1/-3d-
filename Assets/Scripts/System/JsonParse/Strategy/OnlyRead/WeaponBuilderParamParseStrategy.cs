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

        // 从地址中读取到 textasset
        TextAsset textAsset = Resources.Load<TextAsset>(fullPath);
        if (textAsset == null)
        {
            Debug.LogError("没有地址为" + fullPath + "的地址的 textAsset");
            return null;
        }

        // 转为字符串
        string jsontext = textAsset.text;
        if (string.IsNullOrEmpty(jsontext))
        {
            Debug.LogErrorFormat("地址" + fullPath + "所在的地址的配置文件没有配置");
            return null;
        }

        // 初步转为 json 文件
        JsonData jsonData = JsonMapper.ToObject(jsontext);

        // 遍历 JSON 数据
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
