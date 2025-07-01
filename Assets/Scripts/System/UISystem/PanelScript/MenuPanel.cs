using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuPanel : PanelBase
{
    private Transform Play;
    private Transform Option;

    private void Awake()
    {
        Play = transform.Find("BlackPlayButton");
        Option = transform.Find("BlackOptionsButton");
        Play.GetComponent<Button>().onClick.AddListener(OnPlayClick);
        Option.GetComponent<Button>().onClick.AddListener(OnOptionClick);
    }

    private void OnPlayClick()
    {
        TimerManager.MainInstance.TryEnableOneGameTimer(0.2f, () =>
        {
            GameEventManager.MainInstance.CallEvent<string>(EventHash.ChangeSceneState, "BattleState");
        });
    }

    private void OnOptionClick()
    {
        TimerManager.MainInstance.TryEnableOneGameTimer(0.2f, () =>
        {
            //¿ªÆôÃæ°å
            UIManager.MainInstance.OpenPanel(UIConst.OptionsPanel);

        });
    }
}
