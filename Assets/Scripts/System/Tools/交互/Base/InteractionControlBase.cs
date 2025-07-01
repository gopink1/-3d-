using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionControlBase : MonoBehaviour
{
    //灯的控制需要获取灯
    public InteractionBehaviorBase[] interactionBehaviors;

    private bool canInteraction;

    protected virtual void Update()
    {
        Control();
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canInteraction = true;
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canInteraction = false;
        }
    }


    protected virtual void Control()
    {
        if (interactionBehaviors == null) return;
        if (!canInteraction) return;

        if (GameInputManager.MainInstance.Action)
        {
            foreach (var interaction in interactionBehaviors)
            {
                interaction.InterAction();
            }
        }
    }
}
