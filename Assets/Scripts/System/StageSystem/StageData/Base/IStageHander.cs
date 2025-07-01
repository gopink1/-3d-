using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IStageHander
{
    protected IStageData m_stageData = null;      //�ؿ�������
    protected IStageScore m_stageScore = null;    //ͨ�ص�����
    protected IStageHander m_nextHander = null;   //��һ���ؿ�
    
    public IStageHander SetNextHander(IStageHander nextHander)
    {
        m_nextHander = nextHander;
        return m_nextHander;
    }
    public IStageHander GetNextHnader()
    {
        if(m_nextHander == null)
        {
            Debug.Log("m_nextHanderΪ��");
        }
        return m_nextHander;
    }

    /// <summary>
    /// ��ʼ���ؿ�
    /// ���ùؿ����ݵĳ�ʼ������
    /// </summary>
    public abstract void InitStage();
    /// <summary>
    /// ���¹ؿ�
    /// </summary>
    public abstract void UpdateStage();
    /// <summary>
    /// ��ѯ�ؿ�
    /// �鿴�Ƿ�ͨ��
    /// </summary>
    public abstract bool CheckStage();

    public abstract void Release();

    public abstract void ApplyStage( bool apply);
}
