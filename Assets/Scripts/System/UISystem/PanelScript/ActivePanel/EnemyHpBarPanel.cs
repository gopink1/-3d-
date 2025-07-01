using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBarPanel : PanelBase
{
    private Transform Hp;
    private void Awake()
    {
        Hp = transform.Find("Hp");
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

}
