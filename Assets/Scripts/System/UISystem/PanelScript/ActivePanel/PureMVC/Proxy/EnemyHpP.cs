using PureMVC.Patterns.Proxy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����Ѫ����������ݴ���
/// </summary>
public class EnemyHpP : Proxy
{
    //���ݴ��������
    public new const string NAME = "EnemyHpP";

    //�洢��������ֵ��Ϣ����
    private Dictionary<int, EnemyAttr> enemyMap;

    
    public EnemyHpP() : base(EnemyHpP.NAME)
    {
        enemyMap = new Dictionary<int, EnemyAttr>();
    }

    #region ��map���ݵĲ���
    //��������
    public void AddEnemyProxy(int instanceID, EnemyAttr attr)
    {
        if (enemyMap == null)
        {
            enemyMap = new Dictionary<int, EnemyAttr>();
        }
        if (enemyMap.ContainsKey(instanceID))
        {
            Debug.Log("�Ѿ�����id"+instanceID+"�ĵ��˽�����Ϣ���Խ�����Ϣ���и���");
            enemyMap[instanceID] = attr;
        }
        else
        {
            enemyMap.Add(instanceID, attr);
        }
    }
    //ɾ������
    public void RemoveEnemyProxy(int instanceID)
    {
        if (enemyMap.ContainsKey(instanceID))
        {
            enemyMap.Remove(instanceID);
        }
        else
        {
            Debug.Log("������id"+instanceID+"�ĵ��˽�����Ϣ���Ƴ�ʧ��");
        }
    }
    //���
    public void ClearAllData()
    {
        enemyMap.Clear();
    }
    #endregion

    public void UpdateOneEnemyHp(int id,float amount)
    {
        //�������
        //�������
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
