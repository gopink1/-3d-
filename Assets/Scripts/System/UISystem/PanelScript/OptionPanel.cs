using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : PanelBase
{
    private Transform backBtn;
    private Transform homeBtn;
    private Transform volumeSlider;
    private Transform volumeText;

    private void Awake()
    {
        backBtn = transform.Find("BackButton/LeftBlackButton");
        homeBtn = transform.Find("HomeButton/SmallBlackHomeButton");
        volumeSlider = transform.Find("Sound/VolumeSlider");
        volumeText = transform.Find("Sound/Volume");

        InitButtonEvent();
        InitPanelData();
    }


    private void InitButtonEvent()
    {
        backBtn.GetComponent<Button>().onClick.AddListener(OnBackClick);
        homeBtn.GetComponent<Button>().onClick.AddListener(OnHomeClick);
    }

    private void InitPanelData()
    {
        //初始化音量的数据

    }
    private void OnBackClick()
    {
        //返回到主菜单
        TimerManager.MainInstance.TryEnableOneGameTimer(0.1f, () =>
        {
            //关闭当前页面
            UIManager.MainInstance.ClosePanel(UIConst.OptionsPanel);
        });
    }
    private void OnHomeClick()
    {
        //返回到主菜单
        TimerManager.MainInstance.TryEnableOneGameTimer(0.1f, () =>
        {
            //关闭当前页面
            UIManager.MainInstance.ClosePanel(UIConst.OptionsPanel);
        });

    }

}
