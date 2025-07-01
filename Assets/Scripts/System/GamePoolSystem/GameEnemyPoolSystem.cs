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

    private List<PoolItem> configsItem = new List<PoolItem>();//对象池中对象的信息

    private Dictionary<string, Stack<GameObject>> enemyPoolCenter = new Dictionary<string, Stack<GameObject>>();//当前池中存储的对象

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
    /// 对象池的初始化
    /// </summary>
    private void InitPool()
    {
        //初始化Pool，新建一个游戏物体
        //物体中会含有没有进行激活的对象的
        PoolObj = new GameObject();
        PoolObj.name = "GamePool";
    }
    /// <summary>
    /// 添加一个对象池
    /// </summary>
    /// <param name="name">对象池名字</param>
    /// <param name="parentName">池中物体的父类</param>
    /// <param name="list">需要添加入池内的物体</param>
    public void AddOneEnemyPool(string name, string parentName, List<GameObject> list)
    {
        if (list == null) return;
        GameObject parent = null;
        foreach (var obj in list)
        {
            //对缓存对象进行分类
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
            //加入池中
            AddToPool(name, obj);
        }
        //将需要添加的的对象配置信息缓存
        PoolItem item = new PoolItem();
        item.ItemName = name;
        item.Items = list;
        configsItem.Add(item);
        //Debug.Log("名字为" + name + "的对象池对象数量" + enemyPoolCenter[name].Count);
    }
    /// <summary>
    /// 把物体添加入池
    /// </summary>
    /// <param name="name">池名</param>
    /// <param name="obj">对象</param>
    public void AddToPool(string name,GameObject obj) 
    {
        if (!enemyPoolCenter.ContainsKey(name))
        {
            //池中没有的直接新建queue然后添加
            enemyPoolCenter.Add(name, new Stack<GameObject>());
            enemyPoolCenter[name].Push(obj);
            
        }
        else
        {
            //字典中有该队列直接添加
            enemyPoolCenter[name].Push(obj);
        }
    }
    /// <summary>
    /// 获取物体并且在面板中新建
    /// </summary>
    /// <param name="name">获取物体的name</param>
    /// <param name="position">初始化的位置</param>
    /// <param name="rutation">旋转</param>
    public void TryGetPoolItem(string name, Vector3 position, Quaternion rutation)
    {
        if (!enemyPoolCenter.ContainsKey(name))
        {
            Debug.Log("对象池中没有名字为"+ name +"的对象，调用失败");
            return;
        }
        else
        {
            //判断是否为空
            if (enemyPoolCenter[name].Count != 0)
            {
                //Debug.Log("激活对象，栈不为空"+enemyPoolCenter[name].Count);
                //栈有取
                GameObject item = enemyPoolCenter[name].Pop();//字典里有直接取出
                item.transform.position = position;
                item.transform.rotation = rutation;
                item.SetActive(true);
                AddToActiveList(name, item);
            }
            else
            {
                //栈无新
                Debug.Log("对象池燃尽了");
            }
        }
    }
    /// <summary>
    /// 获取物体并且在面板中新建
    /// </summary>
    /// <param name="name">获取物体的name</param>
    /// <param name="position">初始化的位置</param>
    /// <param name="rutation">旋转</param>
    public void TryGetPoolItem(int index, Vector3 position, Quaternion rutation)
    {
        string name = (GameBaseFactory.GetCharacterFactory() as CharacterFactory).GetEnemyName(index);
        if (!enemyPoolCenter.ContainsKey(name))
        {
            Debug.Log("对象池中没有名字为"+ name +"的对象，调用失败");
            return;
        }
        else
        {
            //判断是否为空
            if (enemyPoolCenter[name].Count != 0)
            {
                //Debug.Log("激活对象，栈不为空"+enemyPoolCenter[name].Count);
                //栈有取
                GameObject item = enemyPoolCenter[name].Pop();//字典里有直接取出
                item.transform.position = position;
                item.transform.rotation = rutation;
                item.SetActive(true);
                AddToActiveList(name, item);
            }
            else
            {
                //栈无新
                Debug.Log("对象池燃尽了");
            }
        }
    }
    /// <summary>
    /// 获取物体
    /// </summary>
    /// <returns></returns>
    public GameObject TryGetPoolItem(string name)
    {
        if (enemyPoolCenter.ContainsKey(name))
        {
            var item = enemyPoolCenter[name].Pop();//字典里有直接取出
            item.SetActive(true);
            AddToActiveList(name, item);
            return item;
        }
        Debug.Log("对象池中没有名称为"+name+"的物体");
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


    #region 清空对象池
    /// <summary>
    /// 清楚单个对象池
    /// </summary>
    /// <param name="PoolName"></param>
    public void Clear(string PoolName)
    {
        if (!enemyPoolCenter.ContainsKey(PoolName)) return;
        //清楚池中物体
        foreach(GameObject ob in enemyPoolCenter[PoolName])
        {
            GameObject.Destroy(ob);
        }
        enemyPoolCenter[PoolName].Clear();
        enemyPoolCenter.Remove(PoolName);
        //清除配置信息
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
    /// 回收对象的函数
    /// </summary>
    /// <param name="item"></param>
    public void ReleaseActiveItem(GameObject item)
    {
        //传入的需要回收的对象
        //对象失活
        item.SetActive(false);
        //把对象添加入池中
        string name = item.name;
        if (activeItem.ContainsKey(name))
        {
            activeItem[name].Remove(item);
            AddToPool(name, item);
        }
    }
    /// <summary>
    /// 回收一整个池对象的所有激活对象
    /// </summary>
    /// <param name="activepoolName">想回收池对象的名字</param>
    public void ReleaseOneActivePool(string activepoolName)
    {
        //传入想要回收池对象名字，遍历所有的池对象进行回收
        //回收是所有激活的对象
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
