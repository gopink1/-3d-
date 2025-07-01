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
        //打开加载面板
        LoadingPanel =  UIManager.MainInstance.OpenPanel(UIConst.LoadingPanel) as LoadingPanel;
        //加载场景的开始
        CoroutineHelper.MainInstance.StartCoroutine(LoadSceneAsync());

    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        //更新UI界面
        if (!isLoading) return;
        //当正在异步处理场景时候
        //对进度进行虚假更新
        VirtualAddProgress(Time.deltaTime, 0.02f, 0.1f, 0.1f);
    }

    public override void StateEnd()
    {
        base.StateEnd();
        UIManager.MainInstance.ClosePanel(UIConst.LoadingPanel);
    }


    /// <summary>
    /// 虚假进度条更新
    /// </summary>
    /// <param name="deltaTime">时间增量Time.Deltatime</param>
    /// <param name="timeLimited">进度更新的时间间隔</param>
    /// <param name="addvirtualProgress">虚假进度条的更新的间隔</param>
    /// <param name="interpolition">进度条大于0.9时候的差值增量</param>
    private void VirtualAddProgress(float deltaTime, float timeLimited, float addvirtualProgress, float interpolition)
    {
        timer += deltaTime;
        if (timer> timeLimited)
        {
            timer -= timeLimited;
            //需要更新进度调用panelbase的refresh方法
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
        op.allowSceneActivation = false;//不允许加载完自动启动场景
        isLoading = true;
        //开始异步加载场景，update中进行ui的更新
        bool stop = false;
        while (!stop)
        {
            if(Input.anyKeyDown && isLoading == false) stop = true;
            yield return null;
        }
        //按下下后
        op.allowSceneActivation = true;
        yield return op;
        m_Controler.SetState(new BattleState(m_Controler));
    }
}
