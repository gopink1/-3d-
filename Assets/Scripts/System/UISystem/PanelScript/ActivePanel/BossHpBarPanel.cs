using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBarPanel : PanelBase
{
    //玩家游戏中视角上放的boss血条
    //血条的显示应该在，玩家发现boss（进入一定范围后显示），在远离一定距离后触发血条消失
    private Transform hpPanel;
    private Transform Hp;
    private Transform BossName;
    private void Awake()
    {
        hpPanel = transform.Find("Hp");
        Hp = transform.Find("Hp/Hp");
        BossName = transform.Find("Hp/BossName");
    }
    //传入boss对象名称，设置
    public void SetBoss(string boosName,float hpNum)
    {
        hpPanel.gameObject.SetActive(true);
        BossName.GetComponent<TextMeshProUGUI>().text = boosName;
        Hp.GetComponent<Image>().fillAmount = hpNum;
    }
    public void UpdateHp(float hpRatio)
    {
        // 确保 hpRatio 在 [0.0, 1.0] 范围内
        hpRatio = Mathf.Clamp(hpRatio, -1.0f, 1.0f);

        // 减少 HP 的填充量
        float currentFillAmount = Hp.GetComponent<Image>().fillAmount;
        currentFillAmount += hpRatio;

        // 确保 fillAmount 不低于 0.0
        currentFillAmount = Mathf.Clamp(currentFillAmount, 0.0f, 1.0f);

        // 更新 UI
        Hp.GetComponent<Image>().fillAmount = currentFillAmount;
    }
    public override void OpenPanel(string name)
    {
        base.OpenPanel(name);
        //隐藏面板-当setBoss的时候在进行激活
        hpPanel.gameObject.SetActive(false);
    }
}
