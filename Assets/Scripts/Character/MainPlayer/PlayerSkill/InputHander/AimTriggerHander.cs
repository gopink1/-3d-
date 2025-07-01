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

        //瞄准的责任链处理脚本

        if (data.InputType == InputType.Aim)
        {
            //按下后直接触发技能
            return new InputHanderResult(data,skillInventoryIndex);
        }
        return _nextHandler?.HandleInput(data, skillInventoryIndex);
    }
}
