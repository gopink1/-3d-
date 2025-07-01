using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BossStageData : IStageData
{
   
    private int stageIndex;   //关卡代号，如第几关


    private bool stageStart = false;
    private bool isBorned = false;

    private Vector3 bornPos;
    private Dictionary<int, int> enemyMap;//敌人的字典，包含当前关卡的类型和数量
    private int bossIndex;

    // 记录当前已经生成的敌人数量
    private int currentEnemyCount = 0;
    // 记录上次生成敌人的时间
    private float lastSpawnTime;


    private GameObject boss;
    public BossStageData(Vector3 bornPos,int bossIndex)
    {
        this.bornPos = bornPos;
        this.bossIndex = bossIndex;
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
        CharacterFactory fac = GameBaseFactory.GetCharacterFactory() as CharacterFactory;
        GameObject bossp = fac.CreateBoss(1);
        boss = GameObject.Instantiate(bossp, bornPos, Quaternion.identity);
        boss.SetActive(false);
    }
    public override void Release()
    {
        ////回收当前激活对象返回对象池
        //foreach (var ob in enemyMap.Keys)
        //{
        //    string name = (GameBaseFactory.GetCharacterFactory() as CharacterFactory).GetEnemyName(ob);
        //    //把已激活的对象回收入池
        //    GameBase.MainInstance.ReleaseActivePool(name);
        //}
    }

    #region 敌人生成
    /// <summary>
    /// 根据参数每帧生成敌人
    /// </summary>
    private void ActiveEnemy()
    {
        if (isBorned) return;
        SpawnBoss();
    }

    public void SpawnBoss()
    {
        
        Vector3 spawnPosition;
        // 生成随机位置
        if (bornPos == Vector3.zero)
        {
            spawnPosition = new Vector3(0, 0, 12);
        }
        else
        {
            spawnPosition = bornPos;
        }

        // 从缓存池中取出敌人并激活
        boss.SetActive(true);
        isBorned = true;
        BossHpBarPanel p =  UIManager.MainInstance.GetPanel(UIConst.BossStatePanel) as BossHpBarPanel;

        PMFacade.MainInstance.SendNotification(PMConst.BossNameUpdateCommand, boss.GetComponent<EnemyBossHealthyControl>());
        PMFacade.MainInstance.SendNotification(PMConst.BossHpUpdateCommand,0f);
    }


    #endregion
    public override bool ApplyStage(bool apply)
    {
        stageStart = apply;
        return stageStart;
    }
}
