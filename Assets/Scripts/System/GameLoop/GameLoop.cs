using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{

    SceneStateControler m_SceneStateController = new SceneStateControler(); 

    private void Awake()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameInit();
    }


    // Update is called once per frame
    void Update()
    {
        UpdateGameLogic();
    }
    /// <summary>
    /// ��Ϸ��ʼ��
    /// </summary>
    private void GameInit()
    {
        m_SceneStateController.SetState(new StartState(m_SceneStateController));

        //��״̬ӵ���ߵ�SetState����Ϊ�¼������������Ķ�����е���
        GameEventManager.MainInstance.AddEventListening<string>(EventHash.ChangeSceneState, ChangeSceneState);
    }
    //��Ϸ����
    private void UpdateGameLogic()
    {

        m_SceneStateController.StateUpdate();
    }

    private void ChangeSceneState(string stateName)
    {
        //���ݴ���ĳ������ָı䳡��
        switch (stateName)
        {
            case "StartState":
                m_SceneStateController.SetState(new StartState(m_SceneStateController));
                break;
            case "MainMenuState":
                m_SceneStateController.SetState(new MainMenuState(m_SceneStateController));
                break;
            case "BattleState":
                m_SceneStateController.SetState(new LoadingState(m_SceneStateController,"BattleScene"));
                break;
            default:
                Debug.LogError("�����״̬����");
                break;
        }
    }

    private void OnDestroy()
    {
        GameEventManager.MainInstance.RemoveEvent<string>(EventHash.ChangeSceneState, ChangeSceneState);
    }
}
