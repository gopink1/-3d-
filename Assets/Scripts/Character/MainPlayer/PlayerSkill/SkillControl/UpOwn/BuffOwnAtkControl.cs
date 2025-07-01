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

        //������Ҫ������Ŀ�����λ�õ��ƶ�
        transform.position = lockTarget.position;
    }
}
