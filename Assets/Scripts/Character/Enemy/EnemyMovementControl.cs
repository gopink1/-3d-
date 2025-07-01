using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���˵��ƶ����ƽű�
/// </summary>
public class EnemyMovementControl : CharacterMovementBase
{
    //���˵��ƶ����ƽű�
    //���ڵ��˴�����Ҫ�ƶ�����Ϊʱ����Ҫ���øýű��еķ���
    //1.�����ƶ��߼��ж������Ŀ���
    //2.����ĳЩ״̬ʱ��Ҫ��Խ�ɫ
    [SerializeField] private float MoveSpeed = 2f;


    [SerializeField] private bool ApplyMovement;
    [SerializeField] private bool ApplyRunToTarget = false;
    [SerializeField] private float slerpSpeed = 1f;

    public bool IsApplyMovement
    {
        get => ApplyMovement;
    }
    public bool IsApplyRunToTarget
    {
        get => ApplyRunToTarget;
        set => ApplyRunToTarget = value;
    }
    //������
    private EnemyAttr m_attr = null;

    public void InitHealthyAttr(EnemyAttr attr)
    {
        m_attr = attr;
        MoveSpeed = m_attr.Speed;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    protected override void Update()
    {
        base.Update();

    }
    public void SetApplyMovement(bool apply)
    {
        ApplyMovement = apply;
    }
    /// <summary>
    /// ������Ҫ������Ŀ��λ��
    /// �����Ŀ��
    /// </summary>
    /// <param name="position"></param>
    public void LockViewToTarget(Vector3 position)
    {
        if (ApplyMovement)
        {
            Vector3 lookDir = (position -transform.position).normalized;
            lookDir.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * slerpSpeed);
        }
    }


    /// <summary>
    /// ���ö���������
    /// ������ͨ����Ϊ�����벻ͬ��Ϊ�糯��ɫ��ʱ��Ҫ�ı䶯��������
    /// </summary>
    public void SetAnimatorParameters(float vertical,float horizontal,bool isLocked)
    {
        if (!ApplyMovement) return;
        if(isLocked)
        {
            //�����ƶ���ı���ز���
            animator.SetBool(AnimationHash.HasInputHash, ApplyMovement);
            animator.SetFloat(AnimationHash.LockHash, 1f);
            animator.SetFloat(AnimationHash.MovementHash, 1f);
            animator.SetFloat(AnimationHash.VerticalHash, vertical);
            animator.SetFloat(AnimationHash.HorizontalHash, horizontal);
        }
        else
        {
            //�����ƶ���ı���ز���
            animator.SetBool(AnimationHash.HasInputHash, ApplyMovement);
            animator.SetFloat(AnimationHash.LockHash, 0f);
            animator.SetFloat(AnimationHash.MovementHash, 1f);
            animator.SetFloat(AnimationHash.VerticalHash, vertical);
            animator.SetFloat(AnimationHash.HorizontalHash, horizontal);
        }

    }

    public void MoveToDir(Vector3 dir)
    {
        if(dir != null)
        {
            charactercontroller.Move(dir.normalized * Time.deltaTime * MoveSpeed);
        }
    }
    public void MoveToFor()
    {
        charactercontroller.Move(transform.forward * Time.deltaTime * MoveSpeed);
    }

    private void OnEnemyDead(Transform enemy)
    {
        if (enemy == transform)
        {
            //�������޷��ƶ�
            ApplyMovement = false;
        }
    }
}
