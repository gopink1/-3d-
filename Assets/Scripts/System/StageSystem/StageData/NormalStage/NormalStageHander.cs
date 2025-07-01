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
    /// ��ѯ�ؿ�״̬
    /// �洢��ͨ������
    /// ���ؿ����º�����ж�
    /// </summary>
    public override bool CheckStage()
    {
        return m_stageScore.CheckScore();
    }
    /// <summary>
    /// ���ؿ�����Ϊ��ǰ�ؿ�ʱ�����
    /// </summary>
    public override void InitStage()
    {
        m_stageData.InitStage();
        m_stageScore.InitScore();
    }
    /// <summary>
    /// ���¹ؿ�
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
        //�ͷŵ�ǰ�ؿ��Ļ���
        //������ؽ������
        m_stageData?.Release();
        m_stageScore?.Release();
    }
}
