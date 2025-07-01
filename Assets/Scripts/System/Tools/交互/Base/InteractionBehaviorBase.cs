using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionBehaviorBase : MonoBehaviour,IInteraction
{
    //���пɽ�������Ļ���

    protected bool canInteraction;

    protected virtual void Start()
    {
        canInteraction = true;
    }
    public bool CanInterAction()
    {
        return canInteraction;
    }

    public void InterAction()
    {
        if (!canInteraction) return;
        Interaction();
    }

    protected abstract void Interaction();//�������ʵ�ֵ�
}
