using BehaviorDesigner.Runtime.Tasks.Unity.UnityDebug;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// boss��comboϵͳ����Ϊ����boss����Ҫʹ��״̬��
/// ����ֱ��ʹ����Ϊ������
/// </summary>
public class EnemyBossComboControl : CharacterCombatBase
{
    //����������
    [SerializeField] private Collider[] detectColliders;
    [SerializeField, Header("��ⷶΧ")] private float detecteRange;
    [SerializeField, Header("���Ĳ㼶")] private LayerMask detectLayer;
    [SerializeField, Header("�������")] private float awardDistance;

    [SerializeField, Header("�����о��빥������")] private float mdatkDistance;

    [SerializeField, Header("��������빥������")] private float sdatkDistance;

    [SerializeField] private Dictionary<string, CharacterComboSO> m_ComboSO = new Dictionary<string, CharacterComboSO>();
    [SerializeField] private List<CharacterComboSO> m_ShortCombo = new List<CharacterComboSO>();
    [SerializeField] private List<CharacterComboSO> m_LongCombo = new List<CharacterComboSO>();
    private List<CharacterComboSO> m_currentCombos;
    private bool isLongCombo;

    private bool canChangeCombo;           //�Ƿ����combo�����ڱ궨��ǰ�����Ƿ��Ѿ�ȫ��ִ�У����߱���ϵȣ���Ҫ�л�combo
    public bool CanChangeComb0
    {
        get => canChangeCombo;
    }
    private bool atkCommand;
    public bool AtkCommand
    { 
        get => atkCommand;
        set => atkCommand = value;
    }

    protected override void Start()
    {
        base.Start();
        canChangeCombo = true;

    }
    #region �����������
    //���ڰ�combo���н���������comboso�е����н��з����ʼ��
    public void InitComboSO(Dictionary<string, CharacterComboSO> sos)
    {

        //��ʼ��so��
        m_ComboSO = sos;
        //��so�����ʽ���з���
        foreach(string ci in m_ComboSO.Keys)
        {
            if (ci[3] == 'S')
            {
                m_ShortCombo.Add(m_ComboSO[ci]);
            }
            else if (ci[3] =='L')
            {
                m_LongCombo.Add(m_ComboSO[ci]);
            }
        }
    }

    /// <summary>
    /// �ѵ�ǰ��������Ϊ����������
    /// </summary>
    public void SetComboAsShort()
    {
        if (m_currentCombos == m_ShortCombo) return;
        m_currentCombos = m_ShortCombo;
        isLongCombo = false;
    }
    /// <summary>
    /// �ѵ�ǰ��������ΪԶ����
    /// </summary>
    public void SetComboAsLong()
    {
        if (m_currentCombos == m_LongCombo) return;
        m_currentCombos = m_LongCombo;
        isLongCombo = true;
    }
    /// <summary>
    /// ���һ��combo
    /// </summary>
    public void SetRandomCombo()
    {
        //��ȡ�����
        int randomnum = Random.Range(0, m_currentCombos.Count);
        for(int i = 0; i < m_currentCombos.Count; i++)
        {
            if(i == randomnum)
            {
                currentCombo = m_currentCombos[i];
                canChangeCombo = false;
            }
        }
    }

    
    #endregion

    #region �����������

    #region ִ�й�����Ϊ
    private bool CanAtk()
    {
        if (!canAttack) return false;
        if (!LockPlayer()) return false;
        if (currentEnemy == null) return false;
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit")) return false;
        return true;
    }
    public void BossComboInput()
    {
        if (!CanAtk()) return;

        if(currentCombo == null)
        {
            //Ϊ��˵��δ��ȷ��ʼ��
            Debug.LogError("currentComboΪ�յ���"+gameObject+"����Ϊ��combo��������");
        }
        ExecuteShortComboAction();
    }

    protected override void ExecuteShortComboAction()
    {
        currentComboCount += 1;
        hitIndex = 0;
        //��Ҫ���ж�����ֵ�Ƿ�Ϊ��Χ��
        if (comboIndex >= currentCombo.TryGetComboMaxCount())
        {
            comboIndex = 0;
            currentComboCount = 0;
            //Debug.Log("qqq");
        }
        maxColdTime = currentCombo.TryGetColdTime(comboIndex);//��ȡ��ȴʱ��
        animator.CrossFadeInFixedTime(currentCombo.TryGetOneComboAction(comboIndex), 0.155555f, 0, 0.0f);
        //UpdatePlayerRotate();
        TimerManager.MainInstance.TryEnableOneGameTimer(maxColdTime, UpdateComboInfo);
        canAttack = false;
    }
    protected override void UpdateComboInfo()
    {
        base.UpdateComboInfo();
        comboIndex++;
        maxColdTime = 0;
        canAttack = true;
        if (comboIndex >= currentCombo.TryGetComboMaxCount())
        {
            comboIndex = 0;
            canChangeCombo = true;
            atkCommand = false;
        }
    }
    #endregion

    #region �����ж����

    protected override void TriggerAttack()
    {
        //�����������ж���Ҿ���ͽǶȣ�Ȼ�󴥷������¼�
        if (currentEnemy == null) return;
        if (IsCanAttack(currentEnemy) && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
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
    protected bool IsCanAttack(Transform trans)
    {
        return IsInAttackAngle(trans)&& IsInAttackRange(trans);
    }
    protected bool IsInAttackAngle(Transform trans)
    {

        var dot = Vector3.Dot(transform.forward, GetVector3(trans.position, transform.position));

        if (dot < currentCombo.TryGetComboAngleRange(comboIndex)) return false;//�Ƕȷ�Χ

        //��ȡ�����������еĹ�������
        return true;

    }
    protected bool IsInAttackRange(Transform trans)
    {
        if (GetDistance(trans.transform.position, transform.position) > currentCombo.TryGetComboAtkRange(comboIndex)) return false;//���뷶Χ
        return true;
    }
    #endregion

    #endregion


    #region ��ȡ���˵ķ���
    public bool InAwardRange()
    {
        if (GameBase.MainInstance.GetMainPlayer() == null) return false;
        if (GetDistance(GameBase.MainInstance.GetMainPlayer().transform.position, transform.position) < awardDistance 
            && GetDistance(GameBase.MainInstance.GetMainPlayer().transform.position, transform.position) > mdatkDistance)
        {
            currentEnemy = GameBase.MainInstance.GetMainPlayer().transform;
            return true;
        }
        return false;
    }
    public bool InLongAtkRange()
    {
        if (GameBase.MainInstance.GetMainPlayer() == null) return false;
        if (GetDistance(GameBase.MainInstance.GetMainPlayer().transform.position, transform.position) < mdatkDistance)
        {
            currentEnemy = GameBase.MainInstance.GetMainPlayer().transform;
            return true;
        } 
        return false;
    }
    public bool InShortAtkRange()
    {
        if (GameBase.MainInstance.GetMainPlayer() == null) return false;
        if (GetDistance(GameBase.MainInstance.GetMainPlayer().transform.position, transform.position) < sdatkDistance)
        {
            currentEnemy = GameBase.MainInstance.GetMainPlayer().transform;
            return true;
        }
        return false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position+ transform.up * 0.7f, sdatkDistance);
        Gizmos.DrawWireSphere(transform.position+ transform.up * 0.7f, mdatkDistance);
        Gizmos.DrawWireSphere(transform.position+ transform.up * 0.7f, awardDistance);
    }
    public bool LockPlayer()
    {
        if (GameBase.MainInstance.GetMainPlayer() == null) return false;
        if (GetDistance(GameBase.MainInstance.GetMainPlayer().transform.position, transform.position) < awardDistance)
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
            currentEnemy  = GameBase.MainInstance.GetMainPlayer().transform;
        }
        return currentEnemy;
    }
    #endregion
    protected override void UpdateCharacterRotate()
    {
        if (currentEnemy == null) return;
        //����ƥ����ת
        var currentState = animator.GetCurrentAnimatorStateInfo(0);
        if (currentState.IsTag("Attack") && currentState.normalizedTime < 0.3f)
        {
            // ����Ŀ����ת
            Quaternion targetRotation = Quaternion.LookRotation(currentEnemy.transform.position - transform.position);
            // ƽ����ת��Ŀ����ת
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);

            //Ҫ�����Ǳ��־���
        }
        //Զ������Ҫλ��
        if (currentState.IsTag("Attack")
            && isLongCombo
            && currentState.normalizedTime < currentCombo.TryGetComboMatchTime(comboIndex).y
            && currentState.normalizedTime > currentCombo.TryGetComboMatchTime(comboIndex).x)
        {
            // ����Ŀ����ת
            Quaternion targetRotation = Quaternion.LookRotation(currentEnemy.transform.position - transform.position);
            // ƽ����ת��Ŀ����ת
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);
            Vector3 targetpos = currentEnemy.position + (-transform.forward * currentCombo.TryGetComboPositionOffset(comboIndex));
            //ƥ��λ����Ϣ
            //ƥ�䵽��ɫ��ǰ�ٷ�֮��ʮ����ת�����ϵ���ת*��Է����ټ�ȥ��ǰ���������offset
            //�жϹ��������Ƿ��ٷ�Χ������ڷ�Χ�ڽ���ƥ��

            animator.MatchTarget(targetpos,
                Quaternion.identity,
                AvatarTarget.Root,
                new MatchTargetWeightMask(Vector3.one, 0),
                currentCombo.TryGetComboMatchTime(comboIndex).x,
                currentCombo.TryGetComboMatchTime(comboIndex).y);
        }
    }

}
