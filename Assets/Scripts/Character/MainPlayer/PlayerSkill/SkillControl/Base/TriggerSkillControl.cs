using System;
using UnityEngine;

public class TriggerSkillControl : MonoBehaviour
{
    //触发类型特效控制
    ParticleSystem _particleSystem;
    [SerializeField] float triggerTime;
    [SerializeField] float aliveTime = 1f; // 单次特效存在时间
    [SerializeField] LayerMask enemyLayer;
    private float sectorAngle = 120f;
    private float halfAngle = 60f;
    private float sectorRadius = 5f;


    private float timer = 0f;
    private float maxChargeTime;
    private Vector3 targetPos;//特效目标位置
    private int triggerCount;
    private int currentTriggerIndex = 0; // 当前触发索引

    private float chargeTime = 0f;

    public void Init(Vector3 relativeOffset,Transform emitter, float chargeTime,float maxChargeTime)
    {
        //根据偏移信息和发射者位置计算特效的位置
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



    //播放特效
    private void SetAliveTime()
    {
        float delta = ( maxChargeTime-0.5f ) / 3f;

        //先把蓄力时间进行划分
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
        // 计算当前应该触发的特效索引
        int expectedTriggerIndex = Mathf.FloorToInt((timer - triggerTime) / aliveTime);

        // 确保索引有效且未触发过
        if (expectedTriggerIndex >= 0 && expectedTriggerIndex <= triggerCount && expectedTriggerIndex >= currentTriggerIndex)
        {
            // 触发特效
            _particleSystem.Stop(); // 先停止当前播放
            _particleSystem.Clear(); // 清除现有粒子
            _particleSystem.Play(); // 重新播放


            //延迟对前方扇形物理检测的敌人进行打击
            //HitEnemiesInSector();


            currentTriggerIndex = expectedTriggerIndex + 1; // 更新触发索引
        }
    }

    // 检查是否所有特效都已播放完毕
    private void CheckIfAllTriggersCompleted()
    {
        // 计算所有特效播放完毕的时间点
        float totalDuration = triggerTime + (triggerCount * aliveTime);

        // 如果已过所有特效的总持续时间，销毁对象
        if (timer >= totalDuration)
        {
            Destroy(gameObject);
        }
    }

    //private void HitEnemiesInSector()
    //{
    //    // 获取玩家朝向（假设角色朝向为transform.forward）
    //    Vector3 forwardDirection = transform.forward;

    //    // 计算扇形的左右边界角度
    //    float halfAngle = sectorAngle * 0.5f;
    //    float leftAngle = -halfAngle;
    //    float rightAngle = halfAngle;

    //    // 检测扇形区域内的敌人
    //    Collider[] hits = Physics.OverlapSphere(transform.position, sectorRadius, enemyLayer);

    //    foreach (Collider hit in hits)
    //    {
    //        // 检查敌人是否在扇形角度范围内
    //        if (IsInSector(hit.transform.position, forwardDirection, halfAngle))
    //        {
    //            // 敌人在扇形区域内，执行打击逻辑
    //            CharacterHealthyBase enemyHealth = hit.GetComponent<CharacterHealthyBase>();
    //            if (enemyHealth != null)
    //            {
    //                int damage = 40; // 示例伤害值
    //                GameEventManager.MainInstance.CallEvent(EventHash.OnCharacterHit, damage, "NormalHit", GameBase.MainInstance.GetMainPlayer(), hit.transform);
    //                Debug.Log($"击中敌人！造成 {damage} 点伤害");
    //            }
    //        }
    //    }
    //}

    //// 判断目标位置是否在扇形区域内
    //private bool IsInSector(Vector3 targetPosition, Vector3 forwardDirection, float halfAngle)
    //{
    //    // 计算目标相对于玩家的方向
    //    Vector3 directionToTarget = (targetPosition - transform.position).normalized;

    //    // 计算方向向量与玩家朝向的夹角（角度）
    //    float angle = Vector3.Angle(forwardDirection, directionToTarget);

    //    // 判断角度是否在扇形范围内
    //    return angle <= halfAngle;
    //}
}
