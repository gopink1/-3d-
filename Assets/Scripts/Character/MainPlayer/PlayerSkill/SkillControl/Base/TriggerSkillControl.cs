using System;
using UnityEngine;

public class TriggerSkillControl : MonoBehaviour
{
    //����������Ч����
    ParticleSystem _particleSystem;
    [SerializeField] float triggerTime;
    [SerializeField] float aliveTime = 1f; // ������Ч����ʱ��
    [SerializeField] LayerMask enemyLayer;
    private float sectorAngle = 120f;
    private float halfAngle = 60f;
    private float sectorRadius = 5f;


    private float timer = 0f;
    private float maxChargeTime;
    private Vector3 targetPos;//��ЧĿ��λ��
    private int triggerCount;
    private int currentTriggerIndex = 0; // ��ǰ��������

    private float chargeTime = 0f;

    public void Init(Vector3 relativeOffset,Transform emitter, float chargeTime,float maxChargeTime)
    {
        //����ƫ����Ϣ�ͷ�����λ�ü�����Ч��λ��
        targetPos = emitter.position + emitter.TransformDirection(relativeOffset);
        

        this.chargeTime = chargeTime;
        this.maxChargeTime = maxChargeTime;

        transform.position = targetPos;
        transform.rotation = emitter.rotation;
        SetAliveTime();
    }

    private void Awake()
    {
        timer = 0f;
        _particleSystem = GetComponent<ParticleSystem>();
        _particleSystem.Stop();
    }
    protected virtual void Update()
    {
        timer += Time.deltaTime;
        TriggerEffect();
        CheckIfAllTriggersCompleted();
    }



    //������Ч
    private void SetAliveTime()
    {
        float delta = ( maxChargeTime-0.5f ) / 3f;

        //�Ȱ�����ʱ����л���
        if(chargeTime >= 0f && chargeTime <=delta)
        {
            triggerCount = 1;
        }
        else if(chargeTime > delta && chargeTime <= 2*delta)
        {
            triggerCount = 2;
        }
        else if(chargeTime >2*delta)
        { 
            triggerCount = 3; 
        }
 
    }
    private void TriggerEffect()
    {
        // ���㵱ǰӦ�ô�������Ч����
        int expectedTriggerIndex = Mathf.FloorToInt((timer - triggerTime) / aliveTime);

        // ȷ��������Ч��δ������
        if (expectedTriggerIndex >= 0 && expectedTriggerIndex <= triggerCount && expectedTriggerIndex >= currentTriggerIndex)
        {
            // ������Ч
            _particleSystem.Stop(); // ��ֹͣ��ǰ����
            _particleSystem.Clear(); // �����������
            _particleSystem.Play(); // ���²���


            //�ӳٶ�ǰ������������ĵ��˽��д��
            //HitEnemiesInSector();


            currentTriggerIndex = expectedTriggerIndex + 1; // ���´�������
        }
    }

    // ����Ƿ�������Ч���Ѳ������
    private void CheckIfAllTriggersCompleted()
    {
        // ����������Ч������ϵ�ʱ���
        float totalDuration = triggerTime + (triggerCount * aliveTime);

        // ����ѹ�������Ч���ܳ���ʱ�䣬���ٶ���
        if (timer >= totalDuration)
        {
            Destroy(gameObject);
        }
    }

    //private void HitEnemiesInSector()
    //{
    //    // ��ȡ��ҳ��򣨼����ɫ����Ϊtransform.forward��
    //    Vector3 forwardDirection = transform.forward;

    //    // �������ε����ұ߽�Ƕ�
    //    float halfAngle = sectorAngle * 0.5f;
    //    float leftAngle = -halfAngle;
    //    float rightAngle = halfAngle;

    //    // ������������ڵĵ���
    //    Collider[] hits = Physics.OverlapSphere(transform.position, sectorRadius, enemyLayer);

    //    foreach (Collider hit in hits)
    //    {
    //        // �������Ƿ������νǶȷ�Χ��
    //        if (IsInSector(hit.transform.position, forwardDirection, halfAngle))
    //        {
    //            // ���������������ڣ�ִ�д���߼�
    //            CharacterHealthyBase enemyHealth = hit.GetComponent<CharacterHealthyBase>();
    //            if (enemyHealth != null)
    //            {
    //                int damage = 40; // ʾ���˺�ֵ
    //                GameEventManager.MainInstance.CallEvent(EventHash.OnCharacterHit, damage, "NormalHit", GameBase.MainInstance.GetMainPlayer(), hit.transform);
    //                Debug.Log($"���е��ˣ���� {damage} ���˺�");
    //            }
    //        }
    //    }
    //}

    //// �ж�Ŀ��λ���Ƿ�������������
    //private bool IsInSector(Vector3 targetPosition, Vector3 forwardDirection, float halfAngle)
    //{
    //    // ����Ŀ���������ҵķ���
    //    Vector3 directionToTarget = (targetPosition - transform.position).normalized;

    //    // ���㷽����������ҳ���ļнǣ��Ƕȣ�
    //    float angle = Vector3.Angle(forwardDirection, directionToTarget);

    //    // �жϽǶ��Ƿ������η�Χ��
    //    return angle <= halfAngle;
    //}
}
