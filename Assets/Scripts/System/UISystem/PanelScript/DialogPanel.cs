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
        text.DOText("这是你的牧场\n表面上十分祥和\n十分静谧", 3f);
    }
    public void DoShowText1()
    {
        text.text = "";
        text.DOText("但视角转到牧场门前\n你站在牧场前的平地上\n你感觉到了有敌人来了", 3f);
    }

    public void DoShowText2()
    {
        text.text = "";
        text.DOText("突然出现一个石碑\n你知道这是召唤敌人的石碑\n他们会指引敌人到来\n你需要做的就是把生存下去，然后把Boss击败", 3.5f);
    }


    public void StartDialogPanelEnd()
    {
        //开场动画结束执行
        GameBase.MainInstance.EnterPlaying();
        gameObject.SetActive(false);
    }
}
