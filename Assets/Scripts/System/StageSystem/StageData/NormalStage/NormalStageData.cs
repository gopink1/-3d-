using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NormalStageData : IStageData
{
    
    private int productCd;    //敌人生成的cd
    private int productCount; //敌人单次生成的个数
    private int enemyMaxCount;//当前关卡敌人的上限
    private int stageIndex;   //关卡代号，如第几关

    private bool stageStart = false;

    private Vector2 PosRangeX;//随机敌人位置信息
    private Vector2 PosRangeY;
    private Vector2 PosRangeZ;
    private Dictionary<int, int> enemyMap;//敌人的字典，包含当前关卡的类型和数量


    // 记录当前已经生成的敌人数量
    private int currentEnemyCount = 0;
    // 记录上次生成敌人的时间
    private float lastSpawnTime;

    public NormalStageData(int productCd, int productCount, int enemyMaxCount, int stageIndex, Vector2 posRangeX, Vector2 posRangeY, Vector2 posRangeZ, Dictionary<int, int> enemyMap)
    {
        this.productCd=productCd;
        this.productCount=productCount;
        this.enemyMaxCount=enemyMaxCount;
        this.stageIndex=stageIndex;
        PosRangeX=posRangeX;
        PosRangeY=posRangeY;
        PosRangeZ=posRangeZ;
        this.enemyMap=enemyMap;
    }



    /// <summary>
    /// 初始化关卡
    /// </summary>
    public override void InitStage()
    {
        EnemyCache();

    }
    public override void Update()
    {
        if (!stageStart) return;
        ActiveEnemy();
    }
    /// <summary>
    /// 预载敌人的方法
    /// </summary>
    private void EnemyCache()
    {
        //根据敌人字典enemyMap进行缓存池的预载
        //前清空之前的敌人缓存池
        GameBase.MainInstance.ClearEnemyPool();

        //根据敌人字典进行对敌人预载
        //初始化对象池需要敌人id和数量
        foreach(var ob in enemyMap.Keys)
        {
            GameBase.MainInstance.InitOnePool(ob ,enemyMap[ob]);
        }
    }
    public override void Release()
    {
        //回收当前激活对象返回对象池
        foreach(var ob in enemyMap.Keys)
        {
            string name =  (GameBaseFactory.GetCharacterFactory() as CharacterFactory).GetEnemyName(ob);
            //把已激活的对象回收入池
            GameBase.MainInstance.ReleaseActivePool(name);
        }
    }

    #region 敌人生成
    /// <summary>
    /// 根据参数每帧生成敌人
    /// </summary>
    private void ActiveEnemy()
    {
        // 检查是否开始关卡且当前生成敌人数量未达上限，并且冷却时间已过
        if (currentEnemyCount < enemyMaxCount && Time.time - lastSpawnTime >= productCd)
        {
            SpawnEnemies();
            lastSpawnTime = Time.time;
        }
    }

    private void SpawnEnemies()
    {
        int spawnAmount = Mathf.Min(productCount, enemyMaxCount - currentEnemyCount);
        for (int i = 0; i < spawnAmount; i++)
        {
            // 从敌人字典中获取一种敌人类型
            int enemyType = GetRandomEnemyType();
            if (enemyType != -1)
            {
                // 生成随机位置
                Vector3 spawnPosition = GenerateRandomPosition();
                // 从缓存池中取出敌人并激活
                GameBase.MainInstance.ActiveEnemy(0, spawnPosition);
                currentEnemyCount++;
            }
        }
    }
    private int GetRandomEnemyType()
    {
        List<int> availableEnemyTypes = new List<int>();
        foreach (var kvp in enemyMap)
        {
            if (kvp.Value > 0)
            {
                availableEnemyTypes.Add(kvp.Key);
            }
        }

        if (availableEnemyTypes.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableEnemyTypes.Count);
            int selectedType = availableEnemyTypes[randomIndex];
            enemyMap[selectedType]--;
            return selectedType;
        }
        return -1;
    }

    private Vector3 GenerateRandomPosition()
    {
        float randomX = UnityEngine.Random.Range(PosRangeX.x, PosRangeX.y);
        float randomY = UnityEngine.Random.Range(PosRangeY.x, PosRangeY.y);
        float randomZ = UnityEngine.Random.Range(PosRangeZ.x, PosRangeZ.y);
        return new Vector3(randomX, randomY, randomZ);
    }
    #endregion
    public override bool ApplyStage(bool apply)
    {
        stageStart = apply;
        return stageStart;
    }
}
