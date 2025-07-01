using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyCombatControl : CharacterCombatBase
{
    private FSMachine m_Machine = null;
    //����������
    [SerializeField] private Collider[] detectColliders;
    [SerializeField, Header("��ⷶΧ")] private float detecteRange;
    [SerializeField, Header("���Ĳ㼶")] private LayerMask detectLayer;
    [SerializeField, Header("�������")] private float awardDistance;
    [SerializeField, Header("���빥������")] private float atkDistance;

    [SerializeField, Header("���幥������")] private float atkRange;
    [SerializeField, Header("�����ǶȾ���")] private float atkAngle;
    //��ɫ���б����˵Ĺ���������ɫ״̬�����ƽ�ɫ����Ϊ��ʱ��
    [SerializeField] private Dictionary<string, CharacterComboSO> m_ComboSO = new Dictionary<string, CharacterComboSO>();
    [SerializeField] private CharacterComboSO m_Combo;



    //private EnemyAttr m_attr = null;

    //public void InitHealthyAttr(EnemyAttr attr)
    //{
    //    m_attr = attr;
    //}
    public void InitComboSO(Dictionary<string, CharacterComboSO> sos)
    {
        m_ComboSO = sos;
    }
    public float AwardDistance
    {
        get { return awardDistance; }
    }
    public float AtkDiatance
    {
        get { return atkDistance; }
    }

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        m_Machine.SetState(Enemy_State.Idle);
        //InitMachine();
    }
    public void InitMachines(Dictionary<Enemy_State,FSMState> states,MassageQueue massageQueue)
    {
        //��ʼ��״̬��
        m_Machine = new FSMachine(gameObject, animator, massageQueue);

        //��ʼ��DIC״̬�б�
        foreach (var i in states.Keys)
        {
            states[i].SetMachine(m_Machine);    //����״̬����ÿ��״̬
            m_Machine.AddState(i, states[i]);
        }

    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Update()
    {
        base.Update();
        //�鿴��Ϣ�����е���Ϣ


        if(m_Machine != null)
        {
            m_Machine.StateUpdate();
        }
    }
    public bool LockPlayer()
    {
        if(GameBase.MainInstance.GetMainPlayer() == null) return false;
        if (GetDistance(GameBase.MainInstance.GetMainPlayer().transform.position, transform.position) < AwardDistance)
        {
            currentEnemy = GameBase.MainInstance.GetMainPlayer().transform;
            return true;
        }
        return false;
    }
    public Transform GetCurEnemy()
    {
        if (currentEnemy == null)
        {
            currentEnemy = currentEnemy = GameBase.MainInstance.GetMainPlayer().transform;
        } 
        return currentEnemy;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + transform.up * 0.7f, detecteRange);
    }

    protected override void TriggerAttack()
    {
        //�����������ж���Ҿ���ͽǶȣ�Ȼ�󴥷������¼�
        if (currentEnemy == null) return;
        if(IsCanAttack(currentEnemy) && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            //��������
            //Ŀǰ˵�����˾��ڹ�����Χ�ڣ��������˵�����ϵͳ
            GameEventManager.MainInstance.CallEvent(EventHash.OnCharacterHit,
            currentCombo.TryGetDmage(comboIndex) * atkMultiplier,
            currentCombo.TryGetOneHitComboAction(comboIndex, hitIndex),
            transform,
            currentEnemy.gameObject.transform
            );//��������
            Debug.Log(comboIndex +"-----" + hitIndex);
            Debug.Log(currentCombo.TryGetOneHitComboAction(comboIndex, hitIndex));
        }
    }

    /// <summary>
    /// ִ��������Ϊ
    /// ִ�г���ƽA
    /// </summary>
    public void ExecuteNormalAtkAction()
    {
        //��ѯ��ǰ���
        if (!CanAtk()) return;
        //˵����ǰ�е��˿��Խ���
        //������ҵĶ���
        //����������б����ʽ
        //���һ������

        //���һ������
        int count = m_ComboSO.Count;
        Debug.Log("000000000000000000000000000000000"+count);
        int index = UnityEngine.Random.Range(0, count);
        ChangeComboSO(GetOneCombo(index));


        maxColdTime = currentCombo.TryGetColdTime(comboIndex);
        animator.CrossFadeInFixedTime(currentCombo.TryGetOneComboAction(comboIndex), 0.1555555f, 0, 0.0f);
        TimerManager.MainInstance.TryEnableOneGameTimer(maxColdTime, UpdateComboInfo);
        canAttack = false;
    }
    /// <summary>
    /// �����ȡһ������
    /// </summary>
    public CharacterComboSO GetOneCombo(int index)
    {
        int ci = 0;
        foreach (var combo in m_ComboSO)
        {
            if(ci == index)
            {
                Debug.Log(combo.Value + "���������ʽ����");
                return combo.Value;
            }
            ci++;
        }
        Debug.LogWarning("����û�������ȷ" +  ci + "��������ȷ�ķ�Χ");
        return null;
    }
    private bool CanAtk()
    {
        if (!canAttack) return false;
        if (!LockPlayer()) return false;
        if (currentEnemy == null) return false;
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit")) return false;
        return true;
    }
    protected override void UpdateComboInfo()
    {
        base.UpdateComboInfo();
        comboIndex = 0;
        maxColdTime = 0;
        canAttack = true;
    }
    protected bool IsCanAttack(Transform trans)
    {
        return IsInAttackAngle(trans)&& IsInAttackRange(trans);
    }
    protected bool IsInAttackRange(Transform trans)
    {

        var dot = Vector3.Dot(transform.forward, GetVector3(trans.position, transform.position));

        if (dot < currentCombo.TryGetComboAngleRange(comboIndex)) return false;//�Ƕȷ�Χ

        //��ȡ�����������еĹ�������
        return true;

    }
    protected bool IsInAttackAngle(Transform trans)
    {
        if (GetDistance(trans.transform.position, transform.position) > currentCombo.TryGetComboAtkRange(comboIndex)) return false;//���뷶Χ
        return true;
    }

    /// <summary>
    /// ����״̬��״̬
    /// </summary>
    /// <param name="trans"></param>
    public void SetMachine(Transition trans)
    {
        m_Machine.SetTransition(trans);
    }
}
