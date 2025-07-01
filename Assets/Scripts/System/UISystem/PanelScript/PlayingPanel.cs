using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayingPanel : PanelBase
{
    private Transform StageMassage;
    private Transform AliveTime;
    private Transform PlayerAttr;
    private float needaliveTime;
    private float timer = 0f;
    private int needCount = 0;
    int lastIntTime = 0;

    bool apply =false;

    private Transform Atk;
    private Transform Def;
    private Transform Speed;
    private Transform Exp;
    private void Awake()
    {
        StageMassage = transform.Find("StageMassage");
        AliveTime = transform.Find("AliveTime");
        PlayerAttr = transform.Find("PlayerAttr");
        Atk = transform.Find("PlayerAttr/Atk");
        Def = transform.Find("PlayerAttr/Def");
        Speed = transform.Find("PlayerAttr/Speed");
        Exp = transform.Find("PlayerAttr/Exp");


    }
    public override void OpenPanel(string name)
    {
        base.OpenPanel(name);
        GameEventManager.MainInstance.AddEventListening<int>(EventHash.UpdateStageTimer, SetTimer);
        GameEventManager.MainInstance.AddEventListening<string>(EventHash.UpdateStageText, SetMassage);
    }
    public override void ClosePanel()
    {
        base.ClosePanel();
        GameEventManager.MainInstance.RemoveEvent<int>(EventHash.UpdateStageTimer,SetTimer);
        GameEventManager.MainInstance.RemoveEvent<string>(EventHash.UpdateStageText, SetMassage);
    }
    public void UpdateStateMassage(int index)
    {
        switch (index)
        {
            case 0:
                StageMassage.GetComponent<TextMeshProUGUI>().text = "第一关";
                break;
            case 1:
                StageMassage.GetComponent<TextMeshProUGUI>().text = "第二关";
                break;
            case 2:
                StageMassage.GetComponent<TextMeshProUGUI>().text = "第三关";
                break;
            case 3:
                StageMassage.GetComponent<TextMeshProUGUI>().text = "第四关";
                break;
            case 4:
                StageMassage.GetComponent<TextMeshProUGUI>().text = "第五关";
                break;
            default:
                StageMassage.GetComponent<TextMeshProUGUI>().text = "休整中";
                break;
        }

    }

    public void UpdatePlayerAttr(PlayerAttribute attribute,float value)
    {
        switch (attribute)
        {
            case PlayerAttribute.ATK:
                Atk.GetComponent<TextMeshProUGUI>().text = "Atk: " + value; 
                break;
            case PlayerAttribute.DEF:
                Def.GetComponent<TextMeshProUGUI>().text = "Def: " + value;
                break;
            case PlayerAttribute.SPEED:
                Speed.GetComponent<TextMeshProUGUI>().text = "Speed: " + value;
                break;
            default:
                Debug.LogWarning("cuowu");
                break;
        }
    }

    public void UpdateAliveTime(float value)
    {
        AliveTime.GetComponent<TextMeshProUGUI>().text = value.ToString();
    }


    private void Update()
    {

    }
    public void SetTimer(int needaliveTime)
    {
        AliveTime.GetComponent<TextMeshProUGUI>().text =  needaliveTime.ToString();
    }
    public void SetMassage(string text)
    {
        StageMassage.GetComponent<TextMeshProUGUI>().text = text;
    }


}
