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
        // ��ȡ��ǰ�����
        Image hpImage = Hp.GetComponent<Image>();
        float currentFill = hpImage.fillAmount;

        // �����µ����������ǰֵ + �仯���ʣ�
        float newFill = currentFill + hpRatio;

        // ������[0,1]��Χ��
        newFill = Mathf.Clamp(newFill, 0f, 1f);

        // ����UI
        hpImage.fillAmount = newFill;
    }
    public void UpdateHpText(float curHp,float maxHp)
    {
        // �����ı�
        HpText.GetComponent<TextMeshProUGUI>().text = $"{curHp:F0}|{maxHp:F0}";

        // ����Ѫ�������
        float ratio = maxHp > 0 ? curHp / maxHp : 0f;
        ratio = Mathf.Clamp(ratio, 0f, 1f);
        Hp.GetComponent<Image>().fillAmount = ratio;
    }
}
