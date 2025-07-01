using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttrFactory : IAttrFactory
{
    Dictionary<int, EnemyBaseAttr> m_EnemyAttrDB = null;
    Dictionary<string, WeaponAttr> m_WeaponAttr = null;

    public AttrFactory()
    {
        InitBaseAttr();
        InitWeaponAttr();
    }

    public void InitBaseAttr()
    {
        JsonParseFactory factory = GameBaseFactory.GetJsonParseFactory();
        JsonParseContext<Dictionary<int, EnemyBaseAttr>> jsonParse = factory.CreateParser<Dictionary<int, EnemyBaseAttr>>();
        m_EnemyAttrDB = jsonParse.ParseJsonContext(JsonCfgName.EnemyBaseAttrCfg);
    }
    public void InitWeaponAttr()
    {
        JsonParseFactory factory = GameBaseFactory.GetJsonParseFactory();
        JsonParseContext<Dictionary<string, WeaponAttr>> jsonParse = factory.CreateParser<Dictionary<string, WeaponAttr>>();
        m_WeaponAttr = jsonParse.ParseJsonContext(JsonCfgName.WeaponAttrCfg);

        #region 过去式
        //m_WeaponAttr = new Dictionary<string, WeaponAttr>();
        //m_WeaponAttr.Add("S0_0", new WeaponAttr("S0_0", 10, 1, "Sword0_Green", "Player Sword Animator Controller"));
        //m_WeaponAttr.Add("S0_1", new WeaponAttr("S0_1", 10, 1, "Sword0_Red", "Player Sword Animator Controller"));
        //m_WeaponAttr.Add("S0_2", new WeaponAttr("S0_2", 10, 1, "Sword0_Yellow", "Player Sword Animator Controller"));

        //m_WeaponAttr.Add("BA0_0", new WeaponAttr("BA0_0", 10, 1, "Battleaxe0_Blue", "Player Battleaxe Animator Controller"));
        //m_WeaponAttr.Add("BA0_1", new WeaponAttr("BA0_1", 10, 1, "Battleaxe0_Cyan", "Player Battleaxe Animator Controller"));
        //m_WeaponAttr.Add("BA0_2", new WeaponAttr("BA0_2", 10, 1, "Battleaxe0_Green", "Player Battleaxe Animator Controller"));
        //m_WeaponAttr.Add("BA0_3", new WeaponAttr("BA0_3", 10, 1, "Battleaxe0_Orange", "Player Battleaxe Animator Controller"));
        //m_WeaponAttr.Add("BA0_4", new WeaponAttr("BA0_4", 10, 1, "Battleaxe0_Red", "Player Battleaxe Animator Controller"));
        #endregion
    }

    public override EnemyAttr GetEnemyAttr(int AttrIndex)
    {
        //获取敌人属性
        if (!m_EnemyAttrDB.ContainsKey(AttrIndex))
        {
            Debug.Log("不存在代号为"+AttrIndex+"的敌人基础属性");
            return null;
        }
        EnemyAttr attr = new EnemyAttr();
        attr.SetAttr(m_EnemyAttrDB[AttrIndex]);
        return attr;
    }

    public override WeaponAttr GetWeaponAttr(string AttrIndex)
    {
        if(!m_WeaponAttr.ContainsKey(AttrIndex))
        {
            Debug.Log("不存在代号为"+AttrIndex+"的武器属性");
        }
        return m_WeaponAttr[AttrIndex];
    }
}
