using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanel : PanelBase
{

    [SerializeField]private Text text;

    private void Start()
    {
        DoShowText0();
    }
    public void DoShowText0()
    {
        text.text = "";
        text.DOText("�����������\n������ʮ�����\nʮ�־���", 3f);
    }
    public void DoShowText1()
    {
        text.text = "";
        text.DOText("���ӽ�ת��������ǰ\n��վ������ǰ��ƽ����\n��о������е�������", 3f);
    }

    public void DoShowText2()
    {
        text.text = "";
        text.DOText("ͻȻ����һ��ʯ��\n��֪�������ٻ����˵�ʯ��\n���ǻ�ָ�����˵���\n����Ҫ���ľ��ǰ�������ȥ��Ȼ���Boss����", 3.5f);
    }


    public void StartDialogPanelEnd()
    {
        //������������ִ��
        GameBase.MainInstance.EnterPlaying();
        gameObject.SetActive(false);
    }
}
