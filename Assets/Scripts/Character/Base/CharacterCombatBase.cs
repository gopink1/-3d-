using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterCombatBase : MonoBehaviour
{
    // ������
    // player�͵��� ������ϵͳ�Ļ���

    protected Animator animator;

    protected CharacterComboSO currentCombo;

    protected Transform currentEnemy;


    protected bool canAttack;//�Ƿ�ɹ���
    protected int hitIndex;//�˺�������
    protected float maxColdTime;//�����ȴʱ��
    protected int comboIndex;//���ε�index

    protected float atkMultiplier = 1f;
    public float AtkMultiplier
    {
        get { return atkMultiplier; }
        set { atkMultiplier = value; }
    }

    protected int currentComboCount;//��ǰ�����Ķ����������ж����δ���

    //protected bool isAction;//�Ƿ�����ִ�ж�����


    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }
    protected virtual void Start()
    {
        canAttack = true;
        maxColdTime = 0;
        comboIndex = 0;
        hitIndex = 0;
    }
    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }

    protected virtual void Update()
    {
        RestAttackIndex();
    }
    protected virtual void LateUpdate()
    {
        UpdateCharacterRotate();//��ɫ�����Զ��ӽ���������
    }

    /// <summary>
    /// �ж��ܷ񹥻�
    /// </summary>
    /// <returns> ����һ��boolֵ �����жϽ�ɫ��ǰ��״̬�ܶ�����</returns>
    #region �Ƿ���Թ������

    protected virtual bool CanAttackInput()
    {
        if (!canAttack) return false;
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit")) return false;
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Parry")) return false;
        
        return true;
    }
    #endregion

    #region ��ɫ����
    public virtual void CharacterBaseComboInput() { }
    #endregion
    #region �ı����б�

    protected void ChangeComboSO(CharacterComboSO so)
    {
        if (currentCombo != so)
        {
            currentCombo = so;
            ResetCombo();
        }
    }
    #endregion


    #region ��������
    protected void ResetCombo()
    {
        maxColdTime = 0;
        comboIndex = 0;
        hitIndex = 0;
    }
    #endregion
    #region ����hitindex
    /// <summary>
    /// ���ù�������
    /// �����������������Ƕ���˺�
    /// ÿ�δ���������Ҫ���ж����˴��ŵ�����
    /// </summary>
    protected void ResetHitIndex()
    {
        hitIndex++;
        if (hitIndex == currentCombo.TryGetHitAndParryMaxCount(comboIndex))
        {
            hitIndex = 0;
        }
    }
    #endregion

    #region ִ�ж���
    protected virtual void ExecuteShortComboAction()
    {
    }

    protected virtual void UpdateComboInfo()
    {
    }
    #endregion

    #region ÿ�ι������⵱ǰ״̬�ǲ����ƶ�

    protected void RestAttackIndex()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) return;
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Motion") && !animator.IsInTransition(0))
        {
            ResetCombo();
            currentComboCount = 0;
        }
    }
    #endregion


    protected void ATK()
    {
        TriggerAttack();
        ResetHitIndex();
        //GamePoolManager.MainInstance.TryGetPoolItem("ATKSound", transform.position, transform.rotation);
    }
    protected virtual void TriggerAttack()
    {

    }
    #region ��ȡ����������ĺ���

    protected Vector3 GetVector3(Vector3 v1, Vector3 v2)
    {
        return (v1-v2).normalized;
    }
    protected float GetDistance(Vector3 v1, Vector3 v2)
    {
        return Vector3.Distance(v1, v2);
    }

    #endregion

    /// <summary>
    /// ���½�ɫ�Ķ���ƥ��
    /// </summary>
    protected virtual void UpdateMatchAnimation() { }
    #region ���½�ɫ�ĳ���
    /// <summary>
    /// ��ɫ���ڹ�������ʱ���Զ��ĳ������
    /// </summary>
    protected virtual void UpdateCharacterRotate()
    {
        if (currentEnemy == null) return;
        if (GetDistance(transform.position, currentEnemy.position) > 2f) return;
        var currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.IsTag("Attack") && currentState.normalizedTime < 0.3f)
        {
            // ����Ŀ����ת
            Quaternion targetRotation = Quaternion.LookRotation(currentEnemy.transform.position - transform.position);
            // ƽ����ת��Ŀ����ת
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
    #endregion

}

