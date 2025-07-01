using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeadPanel : PanelBase
{
    Transform title;
    Transform Restart;
    Transform Exit;
    private void Awake()
    {
        title = transform.Find("Text");
        Restart = transform.Find("ReStart");
        Exit = transform.Find("Exit");
        Restart.GetComponent<Button>().onClick.AddListener(OnRestartDown);
        Exit.GetComponent<Button>().onClick.AddListener(OnExitDown);
    }
    private void OnRestartDown()
    {
        GameBase.MainInstance.ReStart();
    }
    private void OnExitDown()
    {
        GameBase.MainInstance.Exit();
    }


    public void SetText(string str)
    {
        title.GetComponent<TextMeshProUGUI>().text = str;
    }
}
