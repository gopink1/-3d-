using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BossStageData : IStageData
{
   
    private int stageIndex;   //�ؿ����ţ���ڼ���


    private bool stageStart = false;
    private bool isBorned = false;

    private Vector3 bornPos;
    private Dictionary<int, int> enemyMap;//���˵��ֵ䣬������ǰ�ؿ������ͺ�����
    private int bossIndex;

    // ��¼��ǰ�Ѿ����ɵĵ�������
    private int currentEnemyCount = 0;
    // ��¼�ϴ����ɵ��˵�ʱ��
    private float lastSpawnTime;


    private GameObject boss;
    public BossStageData(Vector3 bornPos,int bossIndex)
    {
        this.bornPos = bornPos;
        this.bossIndex = bossIndex;
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
        CharacterFactory fac = GameBaseFactory.GetCharacterFactory() as CharacterFactory;
        GameObject bossp = fac.CreateBoss(1);
        boss = GameObject.Instantiate(bossp, bornPos, Quaternion.identity);
        boss.SetActive(false);
    }
    public override void Release()
    {
        ////���յ�ǰ������󷵻ض����
        //foreach (var ob in enemyMap.Keys)
        //{
        //    string name = (GameBaseFactory.GetCharacterFactory() as CharacterFactory).GetEnemyName(ob);
        //    //���Ѽ���Ķ���������
        //    GameBase.MainInstance.ReleaseActivePool(name);
        //}
    }

    #region ��������
    /// <summary>
    /// ���ݲ���ÿ֡���ɵ���
    /// </summary>
    private void ActiveEnemy()
    {
        if (isBorned) return;
        SpawnBoss();
    }

    public void SpawnBoss()
    {
        
        Vector3 spawnPosition;
        // �������λ��
        if (bornPos == Vector3.zero)
        {
            spawnPosition = new Vector3(0, 0, 12);
        }
        else
        {
            spawnPosition = bornPos;
        }

        // �ӻ������ȡ�����˲�����
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
