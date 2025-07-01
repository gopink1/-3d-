using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatePanel : PanelBase
{
    private Transform Hp;
    private Transform Sp;

    private Transform HpText;
    private Transform SpText;
    private void Awake()
    {
        Hp = transform.Find("Hp/Hp");
        Sp = transform.Find("Sp/Sp");
        HpText = transform.Find("HpText");
        SpText = transform.Find("SpText");
    }
    public void UpdateHp(float hpRatio)
    {
        // 获取当前填充量
        Image hpImage = Hp.GetComponent<Image>();
        float currentFill = hpImage.fillAmount;

        // 计算新的填充量（当前值 + 变化比率）
        float newFill = currentFill + hpRatio;

        // 限制在[0,1]范围内
        newFill = Mathf.Clamp(newFill, 0f, 1f);

        // 更新UI
        hpImage.fillAmount = newFill;
    }
    public void UpdateHpText(float curHp,float maxHp)
    {
        // 更新文本
        HpText.GetComponent<TextMeshProUGUI>().text = $"{curHp:F0}|{maxHp:F0}";

        // 更新血条填充量
        float ratio = maxHp > 0 ? curHp / maxHp : 0f;
        ratio = Mathf.Clamp(ratio, 0f, 1f);
        Hp.GetComponent<Image>().fillAmount = ratio;
    }
}
