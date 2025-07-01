using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class StageSystem : IGameSystem
{
    //关卡系统的相关属性
    private int m_NowStageLv = 1;           //当前是第几关
    private int m_EnmemyKilledCount = 0;    //杀敌数量
    private float m_Alivetime = 0.0f;       //当前存活时间
    public float GetAliveTime
    {
        get => m_Alivetime;
    }
    public int lastIntTime = 0;

    private int m_Score;                    //得分
    private bool m_Running = false;                 //是否已经开始

    private IStageHander m_NowStageHander;  //当前关卡
    private IStageHander m_RootStageHander; //起始关卡

    public StageSystem(GameBase gameBase) : base(gameBase)
    {
        Init();
    }

    public override void Init()
    {
        InitAllStageData();
    }

    public override void Release()
    {
        m_Alivetime = 0f;
        m_NowStageHander = null;
        m_RootStageHander = null;
    }

    public override void Update()
    {
        if (!m_Running) return;
        //更新当前关卡内容
        UpdateTimer();
        m_NowStageHander.UpdateStage();
        //判断是否切换关卡
        if (m_NowStageHander.CheckStage())
        {
            //把关卡系统的帧更新停止
            ApplyNowStage(false);
            //关卡结束出现提示词语

            //release当前关卡
            //把当前关卡所有的激活的对象池角色进行回收
            m_NowStageHander.Release();

            StageWin();


            if (m_NowStageHander.GetNextHnader() == null)
            {
                GameBase.MainInstance.GameOver("Win!!!");
                return;
            }
            //获取下一关
            IStageHander newStage = m_NowStageHander.GetNextHnader();
            m_NowStageHander = newStage;
            //通知切换到下一关
            NotifyNewStage();
            Reset();


        }
    }
    /// <summary>
    /// 关卡成功后发放奖励
    /// </summary>
    private void StageWin()
    {
        //召唤出来柱子
        GameEventManager.MainInstance.CallEvent(EventHash.StartStockControl,true);
        //更新UI
        //显示Win的UI FloatingText
        //进入整备状态
        FloatingTextPanel panel = UIManager.MainInstance.OpenPanel(UIConst.FloatingTextPanel) as FloatingTextPanel;
        panel.FloatingText("挑战成功！！！");

        //随机获取属性卡牌
        int c = UnityEngine.Random.Range(0, 3);
        switch (c)
        {
            case 0:
                GameBase.MainInstance.RnadomBornAttrText();
                break;
            case 1:
                GameBase.MainInstance.RandomBornSkillText();
                break;
            case 2:
                GameBase.MainInstance.RandomBornItemText();
                break;
            default:
                break;
        }
    }

    public void UpdateTimer()
    {
        m_Alivetime += Time.deltaTime;
        int currentIntTime = Mathf.FloorToInt(m_Alivetime);
        if (currentIntTime > lastIntTime)
        {
            GameEventManager.MainInstance.CallEvent(EventHash.UpdateStageTimer, currentIntTime);
            Debug.Log("当前的生存时间为" + currentIntTime);
            lastIntTime = currentIntTime;
        }
    }

    private void Reset()
    {
        m_Alivetime = 0;
        lastIntTime = 0;
}
    /// <summary>
    /// 初始化所有关卡
    /// </summary>
    public void InitAllStageData()
    {
        //初始化整个游戏关卡的链表
        //为固定信息，可以通过json文件读取或者配置
        if (m_RootStageHander != null) return;

        NormalStageData StageData = null;
        BossStageData bossStageData = null;
        PlayerAliveStateScore playerAliveStateScore = null;
        EnemyKilledStageScore killStateScore = null;
        NormalStageHander normalStageHander = null;
        Dictionary<int ,int> enemyMap = new Dictionary<int, int>();

        //第一关
        enemyMap.Add(0, 20);
        StageData = new NormalStageData(4, 3, 30, 1,  new UnityEngine.Vector2(0, 30), new UnityEngine.Vector2(0, 1), new UnityEngine.Vector2(0, 30), enemyMap);
        playerAliveStateScore = new PlayerAliveStateScore(5f,this);
        normalStageHander = new NormalStageHander(StageData, playerAliveStateScore);
        //设置为第一关
        m_RootStageHander = normalStageHander;

        //第二关
        enemyMap = new Dictionary<int, int>();
        enemyMap.Add(0, 10);
        StageData = new NormalStageData(3, 4, 40, 1, new UnityEngine.Vector2(0, 30), new UnityEngine.Vector2(0, 30), new UnityEngine.Vector2(0, 30), enemyMap);
        playerAliveStateScore = new PlayerAliveStateScore(10f, this);
        normalStageHander = (NormalStageHander)normalStageHander.SetNextHander(new NormalStageHander(StageData, playerAliveStateScore));

        //第三关
        enemyMap = new Dictionary<int, int>();
        enemyMap.Add(0, 10);
        StageData = new NormalStageData(3, 5, 40, 1, new UnityEngine.Vector2(0, 30), new UnityEngine.Vector2(0, 30), new UnityEngine.Vector2(0, 30), enemyMap);
        playerAliveStateScore = new PlayerAliveStateScore(15f, this);
        normalStageHander = (NormalStageHander)normalStageHander.SetNextHander(new NormalStageHander(StageData, playerAliveStateScore));


        //第四关
        enemyMap = new Dictionary<int, int>();
        enemyMap.Add(0, 20);
        StageData = new NormalStageData(3, 5, 40, 1, new UnityEngine.Vector2(0, 30), new UnityEngine.Vector2(0, 30), new UnityEngine.Vector2(0, 30), enemyMap);
        killStateScore = new EnemyKilledStageScore(0, this,5);
        normalStageHander = (NormalStageHander)normalStageHander.SetNextHander(new NormalStageHander(StageData, killStateScore));


        //第五关
 
        bossStageData = new BossStageData(Vector3.zero,1);
        killStateScore = new EnemyKilledStageScore(1, this,1);
        normalStageHander = (NormalStageHander)normalStageHander.SetNextHander(new NormalStageHander(bossStageData, killStateScore));


    }
    /// <summary>
    /// 预加载关卡
    /// 对当前的关卡进行预加载
    /// </summary>
    public void CacheNowStage()
    {
        m_NowStageHander = m_RootStageHander;
        m_NowStageLv = 1;           //当前是第几关
        m_EnmemyKilledCount = 0;    //杀敌数量
        m_Score = 0;                    //得分

        m_NowStageHander.InitStage();
    }
    public void ResetData()
    {
        m_EnmemyKilledCount = 0;    //杀敌数量
        m_Score = 0;                    //得分
    }
    /// <summary>
    /// 启动当前关卡
    /// 启动当前关卡进行update的更新
    /// </summary>
    public void ApplyNowStage(bool apply)
    {
        //当触发当前函数时候就是开始当前关卡
        m_NowStageHander.ApplyStage(apply);
        //开启游戏触发后下一帧更新就会更新data和score的判断
        m_Running = apply;

        //显示UI
        FloatingTextPanel p = UIManager.MainInstance.OpenPanel(UIConst.FloatingTextPanel) as FloatingTextPanel;
        p.FloatingText("第" + m_NowStageLv +"关 开始");
    }

    /// <summary>
    /// 通知切换新的关卡
    /// 将当前关卡设置为链表下一关卡
    /// </summary>
    public void NotifyNewStage()
    {
        m_NowStageLv++;
        Debug.Log("进入新关卡" + m_NowStageLv);
        //GameEventManager.MainInstance.CallEvent(EventHash.UpdateStageText, m_NowStageLv);
        ResetData();
        m_NowStageHander.InitStage();
    }
}
