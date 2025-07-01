using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTriggerHander : InputHandler
{
    public AimTriggerHander(GameObject obj) : base(obj)
    {
    }

    public override InputHanderResult HandleInput(SkillData data, int skillInventoryIndex)
    {

        //��׼������������ű�

        if (data.InputType == InputType.Aim)
        {
            //���º�ֱ�Ӵ�������
            return new InputHanderResult(data,skillInventoryIndex);
        }
        return _nextHandler?.HandleInput(data, skillInventoryIndex);
    }
}
