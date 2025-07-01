using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseAttrParseStrategy : IJsonParseStrategy<Dictionary<int, EnemyBaseAttr>>
{
    public override Dictionary<int, EnemyBaseAttr> Parse(string fullPath)
    {
        Dictionary<int, EnemyBaseAttr> enemyBaseAttrs = new Dictionary<int, EnemyBaseAttr>();

        // �ӵ�ַ�ж�ȡ��textasset
        TextAsset textAsset = Resources.Load<TextAsset>(fullPath);
        if (textAsset == null)
        {
            Debug.LogError("û�е�ַΪ" + fullPath + "�ĵ�ַ��textAsset");
            return null;
        }

        // תΪ�ַ���
        string jsontext = textAsset.text;
        if (string.IsNullOrEmpty(jsontext))
        {
            Debug.LogErrorFormat("��ַ" + fullPath + "���ڵĵ�ַ�������ļ�û������");
        }

        // ����תΪjson�ļ�
        JsonData jsonData = JsonMapper.ToObject(jsontext);

        // ���� JSON ����
        for (int i = 0; i < jsonData.Count; i++)
        {
            int index = (int)jsonData[i]["index"];
            int maxHp = (int)jsonData[i]["maxHp"];
            int def = (int)jsonData[i]["def"];
            int atk = (int)jsonData[i]["atk"];
            int speed = (int)jsonData[i]["speed"];
            string name = jsonData[i]["name"].ToString();

            EnemyBaseAttr enemyBaseAttr = new EnemyBaseAttr(maxHp, def, atk, speed, name);
            enemyBaseAttrs[index] = enemyBaseAttr;
        }

        return enemyBaseAttrs;
    }

    protected override void InitPath()
    {
        rootPath = "Cfg/";
    }
}
