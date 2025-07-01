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
        // ȷ�� hpRatio �� [0.0, 1.0] ��Χ��
        hpRatio = Mathf.Clamp(hpRatio, -1.0f, 1.0f);

        // ���� HP �������
        float currentFillAmount = Hp.GetComponent<Image>().fillAmount;
        currentFillAmount += hpRatio;

        // ȷ�� fillAmount ������ 0.0
        currentFillAmount = Mathf.Clamp(currentFillAmount, 0.0f, 1.0f);

        // ���� UI
        Hp.GetComponent<Image>().fillAmount = currentFillAmount;
    }

}
