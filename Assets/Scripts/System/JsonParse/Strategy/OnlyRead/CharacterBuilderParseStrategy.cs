using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using TextAsset = UnityEngine.TextAsset;

public class CharacterBuilderParseStrategy : IJsonParseStrategy<Dictionary<int, EnemyBuildParm>>
{
    public override Dictionary<int, EnemyBuildParm> Parse(string fullPath)
    {
        if (string.IsNullOrEmpty(fullPath))
        {
            Debug.Log("路径没有初始化成功为空");
        }
        Dictionary<int, EnemyBuildParm> dic = new Dictionary<int, EnemyBuildParm>();
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
            JsonData enemyParm = data[i];
            int index = (int)enemyParm["index"];
            string name = (string)enemyParm["name"];
            string combosStr = (string)enemyParm["combos"];
            string[] combos = combosStr.Split(',');

            EnemyBuildParm pa = new EnemyBuildParm(index, name);
            foreach (string combo in combos)
            {
                pa.AddComboIndex(combo);
            }

            if (!dic.ContainsKey(index))
            {
                dic.Add(index, pa);
            }
            else
            {
                Debug.LogWarning($"重复的键: {index}，已忽略该条目");
            }
        }
        return dic;
    }

    protected override void InitPath()
    {
        rootPath = "Cfg/";
    }
}
