using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleState : ISceneState
{


    public BattleState(SceneStateControler controler) : base(controler)
    {
        this.StateName = "BattleState";
        this.SceneName = "BattleScene";
    }
    public override void StateBegain()
    {
        Debug.Log(SceneManager.GetSceneByName(SceneName).name);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneName));
        //¿ªÆô
        GameBase.MainInstance.Init();

    }
    public override void StateUpdate()
    {
        GameBase.MainInstance.UpdateSystem();
    }

    public override void StateEnd()
    {
        GameBase.MainInstance.Release();
    }
}
