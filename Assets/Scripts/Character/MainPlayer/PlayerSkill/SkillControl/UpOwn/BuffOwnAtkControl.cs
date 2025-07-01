using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffOwnAtkControl : FollowingSkillControl
{
    private Transform lockTarget;


    public void InitSkill(Transform target)
    {
        lockTarget = target;
    }

    private void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        if (lockTarget == null) return;

        //根据需要锁定的目标进行位置的移动
        transform.position = lockTarget.position;
    }
}
