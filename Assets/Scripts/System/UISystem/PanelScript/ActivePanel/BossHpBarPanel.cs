using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBarPanel : PanelBase
{
    //�����Ϸ���ӽ��Ϸŵ�bossѪ��
    //Ѫ������ʾӦ���ڣ���ҷ���boss������һ����Χ����ʾ������Զ��һ������󴥷�Ѫ����ʧ
    private Transform hpPanel;
    private Transform Hp;
    private Transform BossName;
    private void Awake()
    {
        hpPanel = transform.Find("Hp");
        Hp = transform.Find("Hp/Hp");
        BossName = transform.Find("Hp/BossName");
    }
    //����boss�������ƣ�����
    public void SetBoss(string boosName,float hpNum)
    {
        hpPanel.gameObject.SetActive(true);
        BossName.GetComponent<TextMeshProUGUI>().text = boosName;
        Hp.GetComponent<Image>().fillAmount = hpNum;
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
    public override void OpenPanel(string name)
    {
        base.OpenPanel(name);
        //�������-��setBoss��ʱ���ڽ��м���
        hpPanel.gameObject.SetActive(false);
    }
}
