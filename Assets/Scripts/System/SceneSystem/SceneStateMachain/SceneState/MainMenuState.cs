using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuState : ISceneState
{
    public MainMenuState(SceneStateControler controler) : base(controler)
    {
        this.StateName = "MainMenuState";
        this.SceneName = "MainMenuScene";
    }
    public override void StateBegain()
    {
        //���س���
        SceneManager.LoadScene("MainMenuScene",LoadSceneMode.Additive);
        UIManager.MainInstance.OpenPanel(UIConst.MenuPanel);
    }
    public override void StateUpdate()
    {

    }

    public override void StateEnd()
    {

        UIManager.MainInstance.ClosePanel(UIConst.MenuPanel);
    }
}
