using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHash
{
    public const string HasInputHash = "HasInput";
    public const string RunHash = "Run";
    public const string MovementHash = "Movement";
    public const string LockHash = "Lock";
    public const string HorizontalHash = "Horizontal";
    public const string VerticalHash = "Vertical";
    public const string RoolMoveSpeed = "RoolMoveSpeed";
}
public class CharacterMovementBase : MonoBehaviour
{
    protected Animator animator;
    protected CharacterController charactercontroller;

    protected Vector3 moveDirection;

    //������
    protected bool isGround;//�Ƿ��ڵ���
    [SerializeField, Header("λ��ƫ����")] protected float DetectionPositionOffset;//λ��ƫ����
    [SerializeField, Header("��ⷶΧ")] protected float DetectionRange;//����ƫ����
    [SerializeField, Header("�㼶")] protected LayerMask WhatisGround;//ȷ������Ĳ㼶


    //����
    private float characterGravity = -9.8f;//����
    private float characterVerticalVelocity;//��ֱ�����ϵ��ٶ�
    private float fallOutDalteTime;//�����ӳٵ�ʱ����ж���������¥��ʱ������ص�ʱ�����С����ֵ�򲻻ᴥ������Ķ���
    private float fallOutTime = 0.15f;//�����ӳٵ�ʱ��
    private float maxVerticalVelocity = 54f;
    private Vector3 characterVerticalDirection;


    //�Ƿ���������
    private bool isEnableGravity;




    //�Ƿ����ø��˶�
    protected bool isApplyRootMotion;

    protected virtual void Start()
    {
        fallOutDalteTime = fallOutTime;
        isEnableGravity = true;
        isApplyRootMotion = true;

    }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        charactercontroller = GetComponent<CharacterController>();
    }
    protected virtual void OnEnable()
    {
        GameEventManager.MainInstance.AddEventListening<float>(EventHash.ChangeCharacterVerticalVelocity, ChangeCharacterVerticalVelocity);
        GameEventManager.MainInstance.AddEventListening<bool>(EventHash.EnableGravity, EnableGravity);
    }
    protected virtual void Update()
    {
        SetGravity();
        UpdateVerticalVelocity();
    }
    protected virtual void OnDisable()
    {
        GameEventManager.MainInstance.RemoveEvent<float>(EventHash.ChangeCharacterVerticalVelocity, ChangeCharacterVerticalVelocity);
        GameEventManager.MainInstance.RemoveEvent<bool>(EventHash.EnableGravity, EnableGravity);
    }
    /// <summary>
    /// ����RootMotion�Ľ�ɫ�ƶ�����
    /// </summary>
    protected virtual void OnAnimatorMove()
    {
        animator.ApplyBuiltinRootMotion();
        UpdateCharacterMoveDirection(animator.deltaPosition);
    }
    private bool CheckIsGround()
    {
        Vector3 DetectionPosition = new Vector3(transform.position.x,
            transform.position.y - DetectionPositionOffset, transform.position.z);//ƫ�ƺ�ļ�����ĵ�
        return Physics.CheckSphere(DetectionPosition, //λ��
            DetectionRange, //��Χ
            WhatisGround, //�㼶
            QueryTriggerInteraction.Ignore);//���Դ�����
    }

    private void SetGravity()
    {
        isGround = CheckIsGround();
        if (isGround)
        {
            //�ڵ�����
            fallOutDalteTime = fallOutTime;//������ļ�ʱ����
            //Debug.Log(fallOutDalteTime);
            if (characterVerticalVelocity<0)
            {
                characterVerticalVelocity = -2f;//�ٶȱ�Ϊ-2������֤�´ε���Ծ���ٶ�-2
                //Debug.Log(characterVerticalVelocity);
            }
        }
        else
        {
            if (fallOutDalteTime > 0)
            {
                fallOutDalteTime -= Time.deltaTime;
            }
            else
            {
                //��������ֵ����δ�����棬����������������
            }
            //û���ڵ���
            if (characterVerticalVelocity < maxVerticalVelocity && isEnableGravity)//ʩ���������ϵĸ����µ��ٶ�
            {
                characterVerticalVelocity += characterGravity * Time.deltaTime;
            }
        }
    }

    private void UpdateVerticalVelocity()//���´�ֱ�������ٶ�
    {
        if (!charactercontroller.enabled) return;
        if (isEnableGravity)
        {
            characterVerticalDirection.Set(0, characterVerticalVelocity, 0);
            charactercontroller.Move(characterVerticalDirection  * Time.deltaTime);
        }
    }

    //�µ����
    //����Ƿ����µ���
    private Vector3 SlopResetDirection(Vector3 moveDirection)
    {
        if (!charactercontroller.enabled) return moveDirection;
        if (Physics.Raycast(transform.transform.position + (transform.up * .5f),//���߼��ĳ�ʼֵ
            Vector3.down,//����
            out var hit,//����ֵ
            charactercontroller.height * .85f,//������
            WhatisGround,//�������
            QueryTriggerInteraction.Ignore)//���Դ�����
            )
        {
            if (Vector3.Dot(Vector3.up, hit.normal) != 0)//���ߵ�˲�λ��˵������ֱ��˵�����µ�
            {
                return Vector3.ProjectOnPlane(moveDirection, hit.normal);
            }
        }
        return moveDirection;
    }

    protected virtual void UpdateCharacterMoveDirection(Vector3 moveDir)
    {
        if (!charactercontroller.enabled) return;
        moveDirection = SlopResetDirection(moveDir);
        charactercontroller.Move(moveDirection * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Vector3 DetectionPosition = new Vector3(transform.position.x,
          transform.position.y - DetectionPositionOffset, transform.position.z);//ƫ�ƺ�ļ�����ĵ�
        Gizmos.DrawWireSphere(DetectionPosition, DetectionRange);
    }

    //�¼�ע��
    private void ChangeCharacterVerticalVelocity(float v)
    {
        characterVerticalVelocity = v;
    }

    private void EnableGravity(bool enable)
    {
        isEnableGravity = enable;
    }


}
