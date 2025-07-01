using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttrParseStrategy : IJsonParseStrategy<Dictionary<string, WeaponAttr>>
{

    public override Dictionary<string, WeaponAttr> Parse(string filePath) 
    {
        Dictionary<string, WeaponAttr> weaponAttrDict = new Dictionary<string, WeaponAttr>();

        // 从地址中读取到 TextAsset
        TextAsset textAsset = Resources.Load<TextAsset>(filePath);
        if (textAsset == null)
        {
            Debug.LogError($"没有地址为 {filePath} 的 TextAsset");
            return null;
        }

        // 转为字符串
        string jsonText = textAsset.text;
        if (string.IsNullOrEmpty(jsonText))
        {
            Debug.LogError($"地址 {filePath} 所在的配置文件没有配置");
            return null;
        }

        // 解析 JSON 数据
        JsonData jsonData = JsonMapper.ToObject(jsonText);

        // 遍历 JSON 数组中的每个对象
        for (int i = 0; i < jsonData.Count; i++)
        {
            string weaponIndex = jsonData[i]["weaponIndex"].ToString();
            float atk = (float)jsonData[i]["atk"];
            float range = (float)jsonData[i]["range"];
            string weaponName = jsonData[i]["weaponName"].ToString();
            string animatorName = jsonData[i]["animatorName"].ToString();

            // 创建 WeaponAttr 对象
            WeaponAttr weaponAttr = new WeaponAttr(weaponIndex, atk, range, weaponName, animatorName);

            // 将 WeaponAttr 对象添加到字典中
            weaponAttrDict[weaponIndex] = weaponAttr;
        }

        return weaponAttrDict;
    }

    protected override void InitPath()
    {
        rootPath = "Cfg/";
    }
}
