using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseAttrParseStrategy : IJsonParseStrategy<Dictionary<int, EnemyBaseAttr>>
{
    public override Dictionary<int, EnemyBaseAttr> Parse(string fullPath)
    {
        Dictionary<int, EnemyBaseAttr> enemyBaseAttrs = new Dictionary<int, EnemyBaseAttr>();

        // 从地址中读取到textasset
        TextAsset textAsset = Resources.Load<TextAsset>(fullPath);
        if (textAsset == null)
        {
            Debug.LogError("没有地址为" + fullPath + "的地址的textAsset");
            return null;
        }

        // 转为字符串
        string jsontext = textAsset.text;
        if (string.IsNullOrEmpty(jsontext))
        {
            Debug.LogErrorFormat("地址" + fullPath + "所在的地址的配置文件没有配置");
        }

        // 初步转为json文件
        JsonData jsonData = JsonMapper.ToObject(jsontext);

        // 遍历 JSON 数据
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
