using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockInteractionControl : InteractionControlBase
{
    bool isOpen = false;
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        //进入范围后触发UI提示
        if (isOpen) return;
        UIManager.MainInstance.OpenPanel(UIConst.TriggerMassagePanel);
        isOpen = true;

    }
    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        //进入范围后触发UI提示
        if(!isOpen) return;
        UIManager.MainInstance.ClosePanel(UIConst.TriggerMassagePanel);
        isOpen = false;

    }
}
