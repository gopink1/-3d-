using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PoolType
{
    Enemy,
    Voice
}
[System.Serializable]
public class PoolItem
{
    public string ItemName;
    public List<GameObject> Items;
}
public class GameEnemyPoolSystem : IGameSystem
{
    private GameObject PoolObj = null;

    private List<PoolItem> configsItem = new List<PoolItem>();//������ж������Ϣ

    private Dictionary<string, Stack<GameObject>> enemyPoolCenter = new Dictionary<string, Stack<GameObject>>();//��ǰ���д洢�Ķ���

    private Dictionary<string,List<GameObject>> activeItem = new Dictionary<string,List<GameObject>>();
    public GameEnemyPoolSystem(GameBase gameBase) : base(gameBase)
    {
        Init();
    }
    public override void Init()
    {
        InitPool();
    }
    public override void Release()
    {
        configsItem = null;
        enemyPoolCenter = null;
        activeItem = null;
    }

    public override void Update()
    {
    }
    /// <summary>
    /// ����صĳ�ʼ��
    /// </summary>
    private void InitPool()
    {
        //��ʼ��Pool���½�һ����Ϸ����
        //�����лẬ��û�н��м���Ķ����
        PoolObj = new GameObject();
        PoolObj.name = "GamePool";
    }
    /// <summary>
    /// ���һ�������
    /// </summary>
    /// <param name="name">���������</param>
    /// <param name="parentName">��������ĸ���</param>
    /// <param name="list">��Ҫ�������ڵ�����</param>
    public void AddOneEnemyPool(string name, string parentName, List<GameObject> list)
    {
        if (list == null) return;
        GameObject parent = null;
        foreach (var obj in list)
        {
            //�Ի��������з���
            if (PoolObj.gameObject.transform.Find(parentName) == null)
            {
                parent = new GameObject();
                parent.name = parentName;
                parent.transform.SetParent(PoolObj.transform);
            }
            else
            {
                parent = PoolObj.transform.Find(parentName).gameObject;
            }
            obj.transform.SetParent(parent.transform);
            //�������
            AddToPool(name, obj);
        }
        //����Ҫ��ӵĵĶ���������Ϣ����
        PoolItem item = new PoolItem();
        item.ItemName = name;
        item.Items = list;
        configsItem.Add(item);
        //Debug.Log("����Ϊ" + name + "�Ķ���ض�������" + enemyPoolCenter[name].Count);
    }
    /// <summary>
    /// ������������
    /// </summary>
    /// <param name="name">����</param>
    /// <param name="obj">����</param>
    public void AddToPool(string name,GameObject obj) 
    {
        if (!enemyPoolCenter.ContainsKey(name))
        {
            //����û�е�ֱ���½�queueȻ�����
            enemyPoolCenter.Add(name, new Stack<GameObject>());
            enemyPoolCenter[name].Push(obj);
            
        }
        else
        {
            //�ֵ����иö���ֱ�����
            enemyPoolCenter[name].Push(obj);
        }
    }
    /// <summary>
    /// ��ȡ���岢����������½�
    /// </summary>
    /// <param name="name">��ȡ�����name</param>
    /// <param name="position">��ʼ����λ��</param>
    /// <param name="rutation">��ת</param>
    public void TryGetPoolItem(string name, Vector3 position, Quaternion rutation)
    {
        if (!enemyPoolCenter.ContainsKey(name))
        {
            Debug.Log("�������û������Ϊ"+ name +"�Ķ��󣬵���ʧ��");
            return;
        }
        else
        {
            //�ж��Ƿ�Ϊ��
            if (enemyPoolCenter[name].Count != 0)
            {
                //Debug.Log("�������ջ��Ϊ��"+enemyPoolCenter[name].Count);
                //ջ��ȡ
                GameObject item = enemyPoolCenter[name].Pop();//�ֵ�����ֱ��ȡ��
                item.transform.position = position;
                item.transform.rotation = rutation;
                item.SetActive(true);
                AddToActiveList(name, item);
            }
            else
            {
                //ջ����
                Debug.Log("�����ȼ����");
            }
        }
    }
    /// <summary>
    /// ��ȡ���岢����������½�
    /// </summary>
    /// <param name="name">��ȡ�����name</param>
    /// <param name="position">��ʼ����λ��</param>
    /// <param name="rutation">��ת</param>
    public void TryGetPoolItem(int index, Vector3 position, Quaternion rutation)
    {
        string name = (GameBaseFactory.GetCharacterFactory() as CharacterFactory).GetEnemyName(index);
        if (!enemyPoolCenter.ContainsKey(name))
        {
            Debug.Log("�������û������Ϊ"+ name +"�Ķ��󣬵���ʧ��");
            return;
        }
        else
        {
            //�ж��Ƿ�Ϊ��
            if (enemyPoolCenter[name].Count != 0)
            {
                //Debug.Log("�������ջ��Ϊ��"+enemyPoolCenter[name].Count);
                //ջ��ȡ
                GameObject item = enemyPoolCenter[name].Pop();//�ֵ�����ֱ��ȡ��
                item.transform.position = position;
                item.transform.rotation = rutation;
                item.SetActive(true);
                AddToActiveList(name, item);
            }
            else
            {
                //ջ����
                Debug.Log("�����ȼ����");
            }
        }
    }
    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <returns></returns>
    public GameObject TryGetPoolItem(string name)
    {
        if (enemyPoolCenter.ContainsKey(name))
        {
            var item = enemyPoolCenter[name].Pop();//�ֵ�����ֱ��ȡ��
            item.SetActive(true);
            AddToActiveList(name, item);
            return item;
        }
        Debug.Log("�������û������Ϊ"+name+"������");
        return null;
    }
    private void AddToActiveList(string name,GameObject item)
    {
        if (activeItem.ContainsKey(name))
        {
            activeItem[name].Add(item);
        }
        else
        {
            List<GameObject> list = new List<GameObject>();
            list.Add(item);
            activeItem.Add(name, list);
        }
    }


    #region ��ն����
    /// <summary>
    /// ������������
    /// </summary>
    /// <param name="PoolName"></param>
    public void Clear(string PoolName)
    {
        if (!enemyPoolCenter.ContainsKey(PoolName)) return;
        //�����������
        foreach(GameObject ob in enemyPoolCenter[PoolName])
        {
            GameObject.Destroy(ob);
        }
        enemyPoolCenter[PoolName].Clear();
        enemyPoolCenter.Remove(PoolName);
        //���������Ϣ
        for (int i = configsItem.Count - 1; i >= 0; i--)
        {
            var item = configsItem[i];
            if (item.ItemName == PoolName)
            {
                configsItem.RemoveAt(i);
                break;
            }
        }
    }
    public void Clear()
    {
        for (var i = 0; i<configsItem.Count; i++)
        {
            enemyPoolCenter[configsItem[i].ItemName].Clear();
        }
    }

    
    /// <summary>
    /// ���ն���ĺ���
    /// </summary>
    /// <param name="item"></param>
    public void ReleaseActiveItem(GameObject item)
    {
        //�������Ҫ���յĶ���
        //����ʧ��
        item.SetActive(false);
        //�Ѷ�����������
        string name = item.name;
        if (activeItem.ContainsKey(name))
        {
            activeItem[name].Remove(item);
            AddToPool(name, item);
        }
    }
    /// <summary>
    /// ����һ�����ض�������м������
    /// </summary>
    /// <param name="activepoolName">����ճض��������</param>
    public void ReleaseOneActivePool(string activepoolName)
    {
        //������Ҫ���ճض������֣��������еĳض�����л���
        //���������м���Ķ���
        if(activeItem == null) return;
        if (activeItem.ContainsKey(activepoolName))
        {
            foreach(var item in activeItem[activepoolName])
            {
                ReleaseActiveItem(item);
            }
        }
    }
    #endregion
}
