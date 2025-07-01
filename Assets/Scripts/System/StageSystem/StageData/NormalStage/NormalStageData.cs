using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NormalStageData : IStageData
{
    
    private int productCd;    //�������ɵ�cd
    private int productCount; //���˵������ɵĸ���
    private int enemyMaxCount;//��ǰ�ؿ����˵�����
    private int stageIndex;   //�ؿ����ţ���ڼ���

    private bool stageStart = false;

    private Vector2 PosRangeX;//�������λ����Ϣ
    private Vector2 PosRangeY;
    private Vector2 PosRangeZ;
    private Dictionary<int, int> enemyMap;//���˵��ֵ䣬������ǰ�ؿ������ͺ�����


    // ��¼��ǰ�Ѿ����ɵĵ�������
    private int currentEnemyCount = 0;
    // ��¼�ϴ����ɵ��˵�ʱ��
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
    /// ��ʼ���ؿ�
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
    /// Ԥ�ص��˵ķ���
    /// </summary>
    private void EnemyCache()
    {
        //���ݵ����ֵ�enemyMap���л���ص�Ԥ��
        //ǰ���֮ǰ�ĵ��˻����
        GameBase.MainInstance.ClearEnemyPool();

        //���ݵ����ֵ���жԵ���Ԥ��
        //��ʼ���������Ҫ����id������
        foreach(var ob in enemyMap.Keys)
        {
            GameBase.MainInstance.InitOnePool(ob ,enemyMap[ob]);
        }
    }
    public override void Release()
    {
        //���յ�ǰ������󷵻ض����
        foreach(var ob in enemyMap.Keys)
        {
            string name =  (GameBaseFactory.GetCharacterFactory() as CharacterFactory).GetEnemyName(ob);
            //���Ѽ���Ķ���������
            GameBase.MainInstance.ReleaseActivePool(name);
        }
    }

    #region ��������
    /// <summary>
    /// ���ݲ���ÿ֡���ɵ���
    /// </summary>
    private void ActiveEnemy()
    {
        // ����Ƿ�ʼ�ؿ��ҵ�ǰ���ɵ�������δ�����ޣ�������ȴʱ���ѹ�
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
            // �ӵ����ֵ��л�ȡһ�ֵ�������
            int enemyType = GetRandomEnemyType();
            if (enemyType != -1)
            {
                // �������λ��
                Vector3 spawnPosition = GenerateRandomPosition();
                // �ӻ������ȡ�����˲�����
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
