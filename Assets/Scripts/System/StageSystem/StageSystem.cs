using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class StageSystem : IGameSystem
{
    //�ؿ�ϵͳ���������
    private int m_NowStageLv = 1;           //��ǰ�ǵڼ���
    private int m_EnmemyKilledCount = 0;    //ɱ������
    private float m_Alivetime = 0.0f;       //��ǰ���ʱ��
    public float GetAliveTime
    {
        get => m_Alivetime;
    }
    public int lastIntTime = 0;

    private int m_Score;                    //�÷�
    private bool m_Running = false;                 //�Ƿ��Ѿ���ʼ

    private IStageHander m_NowStageHander;  //��ǰ�ؿ�
    private IStageHander m_RootStageHander; //��ʼ�ؿ�

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
        //���µ�ǰ�ؿ�����
        UpdateTimer();
        m_NowStageHander.UpdateStage();
        //�ж��Ƿ��л��ؿ�
        if (m_NowStageHander.CheckStage())
        {
            //�ѹؿ�ϵͳ��֡����ֹͣ
            ApplyNowStage(false);
            //�ؿ�����������ʾ����

            //release��ǰ�ؿ�
            //�ѵ�ǰ�ؿ����еļ���Ķ���ؽ�ɫ���л���
            m_NowStageHander.Release();

            StageWin();


            if (m_NowStageHander.GetNextHnader() == null)
            {
                GameBase.MainInstance.GameOver("Win!!!");
                return;
            }
            //��ȡ��һ��
            IStageHander newStage = m_NowStageHander.GetNextHnader();
            m_NowStageHander = newStage;
            //֪ͨ�л�����һ��
            NotifyNewStage();
            Reset();


        }
    }
    /// <summary>
    /// �ؿ��ɹ��󷢷Ž���
    /// </summary>
    private void StageWin()
    {
        //�ٻ���������
        GameEventManager.MainInstance.CallEvent(EventHash.StartStockControl,true);
        //����UI
        //��ʾWin��UI FloatingText
        //��������״̬
        FloatingTextPanel panel = UIManager.MainInstance.OpenPanel(UIConst.FloatingTextPanel) as FloatingTextPanel;
        panel.FloatingText("��ս�ɹ�������");

        //�����ȡ���Կ���
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
            Debug.Log("��ǰ������ʱ��Ϊ" + currentIntTime);
            lastIntTime = currentIntTime;
        }
    }

    private void Reset()
    {
        m_Alivetime = 0;
        lastIntTime = 0;
}
    /// <summary>
    /// ��ʼ�����йؿ�
    /// </summary>
    public void InitAllStageData()
    {
        //��ʼ��������Ϸ�ؿ�������
        //Ϊ�̶���Ϣ������ͨ��json�ļ���ȡ��������
        if (m_RootStageHander != null) return;

        NormalStageData StageData = null;
        BossStageData bossStageData = null;
        PlayerAliveStateScore playerAliveStateScore = null;
        EnemyKilledStageScore killStateScore = null;
        NormalStageHander normalStageHander = null;
        Dictionary<int ,int> enemyMap = new Dictionary<int, int>();

        //��һ��
        enemyMap.Add(0, 20);
        StageData = new NormalStageData(4, 3, 30, 1,  new UnityEngine.Vector2(0, 30), new UnityEngine.Vector2(0, 1), new UnityEngine.Vector2(0, 30), enemyMap);
        playerAliveStateScore = new PlayerAliveStateScore(5f,this);
        normalStageHander = new NormalStageHander(StageData, playerAliveStateScore);
        //����Ϊ��һ��
        m_RootStageHander = normalStageHander;

        //�ڶ���
        enemyMap = new Dictionary<int, int>();
        enemyMap.Add(0, 10);
        StageData = new NormalStageData(3, 4, 40, 1, new UnityEngine.Vector2(0, 30), new UnityEngine.Vector2(0, 30), new UnityEngine.Vector2(0, 30), enemyMap);
        playerAliveStateScore = new PlayerAliveStateScore(10f, this);
        normalStageHander = (NormalStageHander)normalStageHander.SetNextHander(new NormalStageHander(StageData, playerAliveStateScore));

        //������
        enemyMap = new Dictionary<int, int>();
        enemyMap.Add(0, 10);
        StageData = new NormalStageData(3, 5, 40, 1, new UnityEngine.Vector2(0, 30), new UnityEngine.Vector2(0, 30), new UnityEngine.Vector2(0, 30), enemyMap);
        playerAliveStateScore = new PlayerAliveStateScore(15f, this);
        normalStageHander = (NormalStageHander)normalStageHander.SetNextHander(new NormalStageHander(StageData, playerAliveStateScore));


        //���Ĺ�
        enemyMap = new Dictionary<int, int>();
        enemyMap.Add(0, 20);
        StageData = new NormalStageData(3, 5, 40, 1, new UnityEngine.Vector2(0, 30), new UnityEngine.Vector2(0, 30), new UnityEngine.Vector2(0, 30), enemyMap);
        killStateScore = new EnemyKilledStageScore(0, this,5);
        normalStageHander = (NormalStageHander)normalStageHander.SetNextHander(new NormalStageHander(StageData, killStateScore));


        //�����
 
        bossStageData = new BossStageData(Vector3.zero,1);
        killStateScore = new EnemyKilledStageScore(1, this,1);
        normalStageHander = (NormalStageHander)normalStageHander.SetNextHander(new NormalStageHander(bossStageData, killStateScore));


    }
    /// <summary>
    /// Ԥ���عؿ�
    /// �Ե�ǰ�Ĺؿ�����Ԥ����
    /// </summary>
    public void CacheNowStage()
    {
        m_NowStageHander = m_RootStageHander;
        m_NowStageLv = 1;           //��ǰ�ǵڼ���
        m_EnmemyKilledCount = 0;    //ɱ������
        m_Score = 0;                    //�÷�

        m_NowStageHander.InitStage();
    }
    public void ResetData()
    {
        m_EnmemyKilledCount = 0;    //ɱ������
        m_Score = 0;                    //�÷�
    }
    /// <summary>
    /// ������ǰ�ؿ�
    /// ������ǰ�ؿ�����update�ĸ���
    /// </summary>
    public void ApplyNowStage(bool apply)
    {
        //��������ǰ����ʱ����ǿ�ʼ��ǰ�ؿ�
        m_NowStageHander.ApplyStage(apply);
        //������Ϸ��������һ֡���¾ͻ����data��score���ж�
        m_Running = apply;

        //��ʾUI
        FloatingTextPanel p = UIManager.MainInstance.OpenPanel(UIConst.FloatingTextPanel) as FloatingTextPanel;
        p.FloatingText("��" + m_NowStageLv +"�� ��ʼ");
    }

    /// <summary>
    /// ֪ͨ�л��µĹؿ�
    /// ����ǰ�ؿ�����Ϊ������һ�ؿ�
    /// </summary>
    public void NotifyNewStage()
    {
        m_NowStageLv++;
        Debug.Log("�����¹ؿ�" + m_NowStageLv);
        //GameEventManager.MainInstance.CallEvent(EventHash.UpdateStageText, m_NowStageLv);
        ResetData();
        m_NowStageHander.InitStage();
    }
}
