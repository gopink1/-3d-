using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����״̬����ͳһ״̬��Ϊ
/// </summary>
public class ISceneState
{
    /// <summary>
    /// ״̬��
    /// </summary>
    protected string m_StateName = "ISceneState";

    /// <summary>
    /// ������
    /// </summary>
    protected string m_SceneName = "";
    public string StateName
    {
        get { return m_StateName; }
        set { m_StateName = value; }
    }
    public string SceneName
    {
        get
        {
            return m_SceneName;
        }
        set 
        {
            m_SceneName = value; 
        }
    }
    /// <summary>
    /// ״̬ӵ����
    /// </summary>
    protected SceneStateControler m_Controler;
    /// <summary>
    /// ���캯����ʼ��״̬ӵ����
    /// </summary>
    /// <param name="controler">״̬ӵ����</param>
    public ISceneState(SceneStateControler controler)
    {
        m_Controler = controler;
    }

    /// <summary>
    /// ״̬��ʼ
    /// </summary>
    public virtual void StateBegain() { }
    /// <summary>
    /// ״̬����
    /// </summary>
    public virtual void StateUpdate() { }
    /// <summary>
    /// ״̬����
    /// </summary>
    public virtual void StateEnd() { }

    protected virtual void LoadCurScene()
    {
        //���ݵ�ǰ�ĳ�������
        //�������س�����Э��
    }
    
}
