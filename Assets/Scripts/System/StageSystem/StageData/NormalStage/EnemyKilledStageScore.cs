using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKilledStageScore : IStageScore
{
    private int bossId;
    private bool getScore;
    private int needCount;
    private int cur_Count;
    public void GetSoure(int id)
    {
        //��Boss��������������֪ͨ����
        if(id ==  bossId)
        {
            cur_Count ++;

        }
    }
    private StageSystem m_stageSys;
    public EnemyKilledStageScore(int bossID, StageSystem stage, int needCount)
    {
        this.bossId = bossID;
        m_stageSys = stage;
        this.needCount = needCount;
    }
    public override bool CheckScore()
    {
        GameEventManager.MainInstance.CallEvent(EventHash.UpdateStageText, "ɱ" + needCount);
        Debug.Log(cur_Count + "��ǰɱ������");
        if(cur_Count >= needCount)
        {
            return true;
        }
        return false;
    }

    public override void InitScore()
    {
        GameEventManager.MainInstance.AddEventListening<int>(EventHash.BossKill, GetSoure);
    }

    public override void Release()
    {
        GameEventManager.MainInstance.RemoveEvent<int>(EventHash.BossKill, GetSoure);
    }
}
