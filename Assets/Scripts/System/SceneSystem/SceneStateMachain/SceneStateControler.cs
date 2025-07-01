using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyTools;

public class SceneStateControler
{
    private ISceneState m_State = null;
    private bool m_Running = false;
    public bool IsRunning
    {
        get { return m_Running; }
    }

    public SceneStateControler() { }

    /// <summary>
    /// ����״̬��״̬(����Ҫ����Loading�ĳ����л�)
    /// </summary>
    /// <param name="state">��Ҫ���ĵ�״̬</param>
    /// <param name="stateName">��Ҫ����״̬��</param>
    public void SetState(ISceneState state)
    {
        ISceneState lastScene = null;
        //��ͬ�����Ĵ�С�ǲ�ͬ�ģ������Ҫ���ص��Ƚϴ�ĳ����ͻ�ʹ�õ�LoadingScene��
        //��sceneName�����жϣ���ҪLoading
        m_Running = false;

        if(m_State != null)
        {
            m_State.StateEnd();
            lastScene = m_State;
        }
        //�л�״̬
        m_State = state;
        //״̬��ʼ
        state.StateBegain();
        m_Running = true;

        if (lastScene != null)
        {
            //����һ������Ϊ���س����Ͳ���Ҫж�س���
            if (lastScene.SceneName == "") return;
            SceneManager.UnloadSceneAsync(lastScene.SceneName);
            Debug.Log("ж�س���" + lastScene.SceneName);
            Debug.Log("��ǰ�����" + SceneManager.GetActiveScene());
        }
    }

    public void StateUpdate()
    {
        if (m_Running)//״̬����������
        {
            //Debug.Log("22222");
            m_State.StateUpdate();
        }
    }
}
