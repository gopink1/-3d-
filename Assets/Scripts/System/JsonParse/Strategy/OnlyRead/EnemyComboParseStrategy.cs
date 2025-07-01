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
            Debug.Log("·��û�г�ʼ���ɹ�Ϊ��");
        }
        Dictionary<string, ComboData> dic = new Dictionary<string, ComboData>();

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
                Debug.LogWarning($"�ظ��ļ�: {comboIndex}���Ѻ��Ը���Ŀ");
            }
        }
        return dic;
    }

    protected override void InitPath()
    {
        rootPath = "Cfg/";
    }
}
