using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttrParseStrategy : IJsonParseStrategy<Dictionary<string, WeaponAttr>>
{

    public override Dictionary<string, WeaponAttr> Parse(string filePath) 
    {
        Dictionary<string, WeaponAttr> weaponAttrDict = new Dictionary<string, WeaponAttr>();

        // �ӵ�ַ�ж�ȡ�� TextAsset
        TextAsset textAsset = Resources.Load<TextAsset>(filePath);
        if (textAsset == null)
        {
            Debug.LogError($"û�е�ַΪ {filePath} �� TextAsset");
            return null;
        }

        // תΪ�ַ���
        string jsonText = textAsset.text;
        if (string.IsNullOrEmpty(jsonText))
        {
            Debug.LogError($"��ַ {filePath} ���ڵ������ļ�û������");
            return null;
        }

        // ���� JSON ����
        JsonData jsonData = JsonMapper.ToObject(jsonText);

        // ���� JSON �����е�ÿ������
        for (int i = 0; i < jsonData.Count; i++)
        {
            string weaponIndex = jsonData[i]["weaponIndex"].ToString();
            float atk = (float)jsonData[i]["atk"];
            float range = (float)jsonData[i]["range"];
            string weaponName = jsonData[i]["weaponName"].ToString();
            string animatorName = jsonData[i]["animatorName"].ToString();

            // ���� WeaponAttr ����
            WeaponAttr weaponAttr = new WeaponAttr(weaponIndex, atk, range, weaponName, animatorName);

            // �� WeaponAttr ������ӵ��ֵ���
            weaponAttrDict[weaponIndex] = weaponAttr;
        }

        return weaponAttrDict;
    }

    protected override void InitPath()
    {
        rootPath = "Cfg/";
    }
}
