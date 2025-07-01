using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFsmParamParseStrategy : IJsonParseStrategy<Dictionary<int, EnemyFSMParm>>
{
    public override Dictionary<int, EnemyFSMParm> Parse(string fullPath)
    {
        Dictionary<int, Dictionary<Enemy_State, Dictionary<Transition, Enemy_State>>> enemyStateTransitions = new Dictionary<int, Dictionary<Enemy_State, Dictionary<Transition, Enemy_State>>>();
        //�ӵ�ַ�ж�ȡ��textasset
        TextAsset textAsset = Resources.Load<TextAsset>(fullPath);
        if (textAsset == null)
        {
            Debug.LogError("û�е�ַΪ"+ fullPath + "�ĵ�ַ��textAsset");
            return null;
        }
        //תΪ�ַ���
        string jsontext = textAsset.text;
        if (String.IsNullOrEmpty(jsontext))
        {
            Debug.LogErrorFormat("��ַ"+ fullPath + "���ڵĵ�ַ�������ļ�û������");
        }
        //����תΪjson�ļ�
        JsonData jsonData = JsonMapper.ToObject<JsonData>(jsontext);
        //��������dic�ĵ�һ��int��
        for (int i = 0; i < jsonData.Count; i++)
        {
            // ��ȡ��������
            int enemyIndex = (int)jsonData[i]["EnemyIndex"];
            // ��ȡ��ǰ״̬
            Enemy_State currentState = (Enemy_State)Enum.Parse(typeof(Enemy_State), jsonData[i]["State"].ToString());
            // ��ȡ״̬ת������
            Transition transition = (Transition)Enum.Parse(typeof(Transition), jsonData[i]["Transition"].ToString());
            // ��ȡת�����״̬
            Enemy_State transToState = (Enemy_State)Enum.Parse(typeof(Enemy_State), jsonData[i]["TransToState"].ToString());

            // ����õ���������δ���ֵ��У����г�ʼ��
            if (!enemyStateTransitions.ContainsKey(enemyIndex))
            {
                enemyStateTransitions[enemyIndex] = new Dictionary<Enemy_State, Dictionary<Transition, Enemy_State>>();
            }

            // �����ǰ״̬��δ���ֵ��У����г�ʼ��
            if (!enemyStateTransitions[enemyIndex].ContainsKey(currentState))
            {
                enemyStateTransitions[enemyIndex][currentState] = new Dictionary<Transition, Enemy_State>();
            }

            // ���״̬ת����Ϣ
            enemyStateTransitions[enemyIndex][currentState][transition] = transToState;
        }
        // �������յĵ���״̬�������ֵ�
        Dictionary<int, EnemyFSMParm> m_enemyFSM = new Dictionary<int, EnemyFSMParm>();
        foreach (var kvp in enemyStateTransitions)
        {
            m_enemyFSM[kvp.Key] = new EnemyFSMParm(kvp.Value);
        }
        return m_enemyFSM;
    }

    protected override void InitPath()
    {
        rootPath = "Cfg/";
    }
}
