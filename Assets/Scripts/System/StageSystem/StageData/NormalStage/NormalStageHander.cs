using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalStageHander : IStageHander
{
    
    public IStageScore ScoreType
    {
        get =>m_stageScore;
    }
    public IStageData StageData
    {
        get => m_stageData;
    }
    public NormalStageHander(IStageData data,
                             IStageScore score)
    {
        m_stageData = data;
        m_stageScore = score;
    }
    /// <summary>
    /// 查询关卡状态
    /// 存储着通关条件
    /// 当关卡更新后进行判断
    /// </summary>
    public override bool CheckStage()
    {
        return m_stageScore.CheckScore();
    }
    /// <summary>
    /// 当关卡设置为当前关卡时候调用
    /// </summary>
    public override void InitStage()
    {
        m_stageData.InitStage();
        m_stageScore.InitScore();
    }
    /// <summary>
    /// 更新关卡
    /// </summary>
    public override void UpdateStage()
    {
        m_stageData.Update();

    }
    public override void ApplyStage(bool apply)
    {
        m_stageData.ApplyStage(apply);
    }

    public override void Release()
    {
        //释放当前关卡的缓存
        //将对象池进行清空
        m_stageData?.Release();
        m_stageScore?.Release();
    }
}
