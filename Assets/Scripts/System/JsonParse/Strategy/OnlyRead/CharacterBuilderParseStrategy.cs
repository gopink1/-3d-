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
            Debug.Log("·��û�г�ʼ���ɹ�Ϊ��");
        }
        Dictionary<int, EnemyBuildParm> dic = new Dictionary<int, EnemyBuildParm>();
        // ���ļ��ж�ȡ�ļ���Ϣ
        TextAsset textAsset = Resources.Load<TextAsset>(fullPath);
        if (textAsset == null)
        {
            Debug.LogError($"δ���ҵ���Դ�ļ�: {fullPath}");
            return dic;
        }
        // �Ѷ�ȡ�ļ���Ϣ
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
                Debug.LogWarning($"�ظ��ļ�: {index}���Ѻ��Ը���Ŀ");
            }
        }
        return dic;
    }

    protected override void InitPath()
    {
        rootPath = "Cfg/";
    }
}
