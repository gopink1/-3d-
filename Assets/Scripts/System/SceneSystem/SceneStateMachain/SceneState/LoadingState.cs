using MyTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingState : ISceneState
{
    private string nextSceneName;

    private LoadingPanel LoadingPanel;
    private float timer = 0;
    private float virtualProgress;
    private bool isLoading = false;
    public LoadingState(SceneStateControler controler,string nextScene) : base(controler)
    {
        this.StateName = "LoadingScene";
        nextSceneName = nextScene;
    }

    public override void StateBegain()
    {
        base.StateBegain();
        //�򿪼������
        LoadingPanel =  UIManager.MainInstance.OpenPanel(UIConst.LoadingPanel) as LoadingPanel;
        //���س����Ŀ�ʼ
        CoroutineHelper.MainInstance.StartCoroutine(LoadSceneAsync());

    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        //����UI����
        if (!isLoading) return;
        //�������첽������ʱ��
        //�Խ��Ƚ�����ٸ���
        VirtualAddProgress(Time.deltaTime, 0.02f, 0.1f, 0.1f);
    }

    public override void StateEnd()
    {
        base.StateEnd();
        UIManager.MainInstance.ClosePanel(UIConst.LoadingPanel);
    }


    /// <summary>
    /// ��ٽ���������
    /// </summary>
    /// <param name="deltaTime">ʱ������Time.Deltatime</param>
    /// <param name="timeLimited">���ȸ��µ�ʱ����</param>
    /// <param name="addvirtualProgress">��ٽ������ĸ��µļ��</param>
    /// <param name="interpolition">����������0.9ʱ��Ĳ�ֵ����</param>
    private void VirtualAddProgress(float deltaTime, float timeLimited, float addvirtualProgress, float interpolition)
    {
        timer += deltaTime;
        if (timer> timeLimited)
        {
            timer -= timeLimited;
            //��Ҫ���½��ȵ���panelbase��refresh����
            if (virtualProgress < 0.9f)
            {
                virtualProgress += addvirtualProgress;
                LoadingPanel.AddProgress(addvirtualProgress);
                //Debug.Log(virtualProgress);
            }
            else
            {
                virtualProgress = Mathf.Lerp(virtualProgress, 1f, interpolition);
                LoadingPanel.RefreshProgress(virtualProgress);
                if (virtualProgress >= 0.99f)
                {
                    LoadingPanel.RefreshProgress(virtualProgress);
                    Debug.Log(virtualProgress);
                    LoadingPanel.SetProgressText("press any key");
                    isLoading = false;
                }
            }

        }
    }


    IEnumerator LoadSceneAsync()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        op.allowSceneActivation = false;//������������Զ���������
        isLoading = true;
        //��ʼ�첽���س�����update�н���ui�ĸ���
        bool stop = false;
        while (!stop)
        {
            if(Input.anyKeyDown && isLoading == false) stop = true;
            yield return null;
        }
        //�����º�
        op.allowSceneActivation = true;
        yield return op;
        m_Controler.SetState(new BattleState(m_Controler));
    }
}
