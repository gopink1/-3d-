using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class InstantTriggerHander : InputHandler
{
    public InstantTriggerHander(GameObject obj) : base(obj)
    {
    }

    public override InputHanderResult HandleInput(SkillData data, int skillInventoryIndex)
    {
        if (data.InputType == InputType.Instant)
        {
            //���º�ֱ�Ӵ�������
            return new InputHanderResult(data,skillInventoryIndex);
        }
        return _nextHandler?.HandleInput(data,skillInventoryIndex);
    }
}
