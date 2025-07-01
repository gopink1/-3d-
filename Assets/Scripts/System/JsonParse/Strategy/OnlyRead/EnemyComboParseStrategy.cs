using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyComboParseStrategy : IJsonParseStrategy<Dictionary<string, ComboData>>
{
    public override Dictionary<string, ComboData> Parse(string fullPath)
    {
        if (string.IsNullOrEmpty(fullPath))
        {
            Debug.Log("路径没有初始化成功为空");
        }
        Dictionary<string, ComboData> dic = new Dictionary<string, ComboData>();

        // 从文件中读取文件信息
        TextAsset textAsset = Resources.Load<TextAsset>(fullPath);
        if (textAsset == null)
        {
            Debug.LogError($"未能找到资源文件: {fullPath}");
            return dic;
        }

        // 把读取文件信息
        string text = textAsset.text;
        JsonData data = JsonMapper.ToObject(text);

        for (int i = 0; i < data.Count; i++)
        {
            JsonData comboData = data[i];
            string comboIndex = (string)comboData["comboIndex"];
            string comboName = (string)comboData["comboName"];
            string comboPath = (string)comboData["comboPath"];

            ComboData cd = new ComboData(comboIndex, comboName, comboPath);
            if (!dic.ContainsKey(comboIndex))
            {
                dic.Add(comboIndex, cd);
            }
            else
            {
                Debug.LogWarning($"重复的键: {comboIndex}，已忽略该条目");
            }
        }
        return dic;
    }

    protected override void InitPath()
    {
        rootPath = "Cfg/";
    }
}
