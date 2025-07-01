using PureMVC.Patterns.Proxy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敌人血条管理的数据代理
/// </summary>
public class EnemyHpP : Proxy
{
    //数据代理的名字
    public new const string NAME = "EnemyHpP";

    //存储敌人生命值信息的类
    private Dictionary<int, EnemyAttr> enemyMap;

    
    public EnemyHpP() : base(EnemyHpP.NAME)
    {
        enemyMap = new Dictionary<int, EnemyAttr>();
    }

    #region 对map数据的操作
    //增加数据
    public void AddEnemyProxy(int instanceID, EnemyAttr attr)
    {
        if (enemyMap == null)
        {
            enemyMap = new Dictionary<int, EnemyAttr>();
        }
        if (enemyMap.ContainsKey(instanceID))
        {
            Debug.Log("已经存在id"+instanceID+"的敌人健康信息，对健康信息进行更新");
            enemyMap[instanceID] = attr;
        }
        else
        {
            enemyMap.Add(instanceID, attr);
        }
    }
    //删除数据
    public void RemoveEnemyProxy(int instanceID)
    {
        if (enemyMap.ContainsKey(instanceID))
        {
            enemyMap.Remove(instanceID);
        }
        else
        {
            Debug.Log("不存在id"+instanceID+"的敌人健康信息，移除失败");
        }
    }
    //清空
    public void ClearAllData()
    {
        enemyMap.Clear();
    }
    #endregion

    public void UpdateOneEnemyHp(int id,float amount)
    {
        //计算比率
        //计算比率
        float radio = (float)(amount / enemyMap[id].MaxHP);
        PMFacade.MainInstance.SendNotification(PMConst.UpdateEHpToView, new object[]{
        id,//string
        radio//float
        } );
    }
    public override void OnRemove()
    {
        base.OnRemove();
        ClearAllData();
    }
}
