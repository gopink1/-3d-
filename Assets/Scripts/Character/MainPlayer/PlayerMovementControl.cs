using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementControl : CharacterMovementBase
{
    [SerializeField]private PlayerAttr m_attr;

    #region ��ɫ�ƶ���ת��
    private Camera m_Camera;
    private float _rotateAngle;//ת��Ƕ�

    private float currentVelocity ;

    [SerializeField] private float smoothTime;

    protected override void Start()
    {
        base.Start();
        m_attr = GetComponent<PlayerHealthyControl>().GetAttr();
    }

    private void CharacterRotateControl()
    {
        if (animator.GetBool(AnimationHash.HasInputHash))
        {
            _rotateAngle = Mathf.Atan2(GameInputManager.MainInstance.Movement.x,
           GameInputManager.MainInstance.Movement.y) * Mathf.Rad2Deg +
           m_Camera.transform.eulerAngles.y;  //��ɫ�������ת
        }
        if (animator.GetBool(AnimationHash.HasInputHash) && (animator.GetCurrentAnimatorStateInfo(0).IsTag("Motion")||animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")))
        {
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y,
            _rotateAngle,
            ref currentVelocity,
            smoothTime);
        }
    }

    /// <summary>
    /// ����������¶���
    /// </summary>
    private void UpdateAnimation()
    {
        if (!isGround) return;

        animator.SetBool(AnimationHash.HasInputHash, GameInputManager.MainInstance.Movement != Vector2.zero);
        if (animator.GetBool(AnimationHash.HasInputHash))
        {
            animator.SetBool(AnimationHash.RunHash, GameInputManager.MainInstance.Run);
            animator.SetFloat(AnimationHash.MovementHash,
            animator.GetBool(AnimationHash.RunHash) ? 2f : GameInputManager.MainInstance.Movement.sqrMagnitude,
                    .25f,
                    Time.deltaTime);

            animator.SetFloat(AnimationHash.HorizontalHash,
                        0f,
                        .25f,
                        Time.deltaTime);
            animator.SetFloat(AnimationHash.VerticalHash,
                        0f,
                        .25f,
                        Time.deltaTime);
        }
        else
        {
            animator.SetFloat(AnimationHash.MovementHash, 0,
            .25f,
            Time.deltaTime);
            if (animator.GetFloat(AnimationHash.MovementHash) < 0.2f)
            {
                animator.SetBool(AnimationHash.RunHash, false);
            }
        }

    }

    protected override void OnAnimatorMove()
    {
        if (!isApplyRootMotion) return;
        animator.ApplyBuiltinRootMotion();
        UpdateCharacterMoveDirection(animator.deltaPosition * m_attr.Speed);
    }
    #endregion

    #region ��ɫ����
    //��ɫ������ز���
    private float timer;
    [SerializeField] private float roolMoveSpeed;

    /// <summary>
    /// ��ɫ��������߼�
    /// </summary>
    private void PlayerDodgeUpdate()
    {
        //������ȴ�¼������δ��������
        ResetRoolTimer();
        //��ɫ���ܵĴ���
        //���������ܺ�ᴥ��
        if (!animator) return;
        if (!CanDodge()) return;
        //δ����
        if (GameInputManager.MainInstance.Dodge)
        {
            animator.Play("Dodge");
            ////Ȼ����Ը��ݼ�ʱ��Ҳ�Ϳ��Ը��ݶ����¼����ûظ����˶�
            TimerManager.MainInstance.TryEnableOneGameTimer(0.75f, OnRoolEnd);
            RemoveRootMotion();
        }
    }
    private bool CanDodge()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) return false;
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Finality")) return false;
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit")) return false;
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Assassinate")) return false;
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Climb")) return false;
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Dodge")) return false;
        if(animator.IsInTransition(0)) return false;

        if (timer != 0f) return false;
        return true;
    }

    private void UpdateDodgeMoveDir()
    {
        //��Ϊ�������������û������
        //δ��������򵥵���ǰ����
        //������Ҫ�����뷽���������ĳ�������ж϶������λ��

        //�ƶ��ľ���ͨ�������������ٶ�
        //����UpdateCharacterMoveDirection�������з�����λ�ƴ�������ݿ��Կ����ٶ�
        if ((animator.GetFloat(AnimationHash.LockHash) == 0))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Dodge"))
            {
                timer += Time.deltaTime;
                //Debug.Log(timer);
                //δ����״̬
                //roolMoveSpeed = animator.GetFloat(AnimationHash.RoolMoveSpeed);

                UpdateCharacterMoveDirection(transform.forward  * roolMoveSpeed * 2.5f * animator.GetFloat("DodgeOffset"));
            }
        }
        else if ((animator.GetFloat(AnimationHash.LockHash) == 1))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Dodge") && !animator.IsInTransition(0))
            {
                //δ����״̬
                timer += Time.deltaTime;
                //����������ж��������ж�
                //����������������ж�λ�Ʒ���
                Vector3 dir = new Vector3(animator.GetFloat(AnimationHash.HorizontalHash), 0, animator.GetFloat(AnimationHash.VerticalHash));//��ȡ������������ķ���
                //��Ҫ�ѷ�����ת
                Quaternion rotation = Quaternion.Euler(0, _rotateAngle, 0);
                dir = rotation * dir;
                UpdateCharacterMoveDirection(dir * 6f);
            }
        }


    }

    private void ResetRoolTimer()
    {
        if (timer == 0) return;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Dodge"))
        {
            timer = 0f;
        }
    }

    /// <summary>
    /// ���ø��˶�
    /// </summary>
    private void RemoveRootMotion()
    {
        isApplyRootMotion = false;
        animator.applyRootMotion = false;
    }
    /// <summary>
    /// ���ø��˶�
    /// </summary>
    private void OnRoolEnd()
    {
        isApplyRootMotion = true;
        animator.applyRootMotion = true;
        timer = 0f;
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        //m_Camera = Camera.main;
        m_Camera = GameObject.FindWithTag("PlayerCamera").transform.GetComponent<Camera>();
    }

    protected override void Update()
    {
        base.Update();
        PlayerDodgeUpdate();
        UpdateDodgeMoveDir();
    }
    private void LateUpdate()
    {
        UpdateAnimation();
        CharacterRotateControl();
        //UpdateCharacterFootSound();
    }
    protected override void OnEnable()
    {
        base.OnEnable();

    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }



    //private void UpdateCharacterFootSound()
    //{
    //    //���ڵ������沢�����ƶ����������ƶ��ٶȴﵽ0.5ʱ�Żᴥ���ƶ�������
    //    if (isGround && animator.GetCurrentAnimatorStateInfo(0).IsTag("Motion") && animator.GetFloat(AnimationHash.MovementHash) > .5f)
    //    {
    //        nextFootSound -= Time.deltaTime;
    //        if (nextFootSound < 0f)
    //        {
    //            PlayFootSound();
    //        }
    //    }
    //    else
    //    {
    //        nextFootSound = 0f;
    //    }
    //}

    //private void PlayFootSound()
    //{
    //    GamePoolManager.MainInstance.TryGetPoolItem("FootSound", transform.position, transform.rotation);
    //    nextFootSound = animator.GetFloat(AnimationHash.MovementHash)>1.1f ? fastFootSound : slowFootSound;
    //}


}
