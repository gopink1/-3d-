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
        //从地址中读取到textasset
        TextAsset textAsset = Resources.Load<TextAsset>(fullPath);
        if (textAsset == null)
        {
            Debug.LogError("没有地址为"+ fullPath + "的地址的textAsset");
            return null;
        }
        //转为字符串
        string jsontext = textAsset.text;
        if (String.IsNullOrEmpty(jsontext))
        {
            Debug.LogErrorFormat("地址"+ fullPath + "所在的地址的配置文件没有配置");
        }
        //初步转为json文件
        JsonData jsonData = JsonMapper.ToObject<JsonData>(jsontext);
        //遍历提起dic的第一个int键
        for (int i = 0; i < jsonData.Count; i++)
        {
            // 获取敌人索引
            int enemyIndex = (int)jsonData[i]["EnemyIndex"];
            // 获取当前状态
            Enemy_State currentState = (Enemy_State)Enum.Parse(typeof(Enemy_State), jsonData[i]["State"].ToString());
            // 获取状态转换条件
            Transition transition = (Transition)Enum.Parse(typeof(Transition), jsonData[i]["Transition"].ToString());
            // 获取转换后的状态
            Enemy_State transToState = (Enemy_State)Enum.Parse(typeof(Enemy_State), jsonData[i]["TransToState"].ToString());

            // 如果该敌人索引还未在字典中，进行初始化
            if (!enemyStateTransitions.ContainsKey(enemyIndex))
            {
                enemyStateTransitions[enemyIndex] = new Dictionary<Enemy_State, Dictionary<Transition, Enemy_State>>();
            }

            // 如果当前状态还未在字典中，进行初始化
            if (!enemyStateTransitions[enemyIndex].ContainsKey(currentState))
            {
                enemyStateTransitions[enemyIndex][currentState] = new Dictionary<Transition, Enemy_State>();
            }

            // 添加状态转换信息
            enemyStateTransitions[enemyIndex][currentState][transition] = transToState;
        }
        // 创建最终的敌人状态机参数字典
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
