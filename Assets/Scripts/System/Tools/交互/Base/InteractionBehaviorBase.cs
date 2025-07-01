using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionBehaviorBase : MonoBehaviour,IInteraction
{
    //所有可交互物体的基类

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

    protected abstract void Interaction();//子类必须实现的
}
