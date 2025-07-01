using MyTools;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChargeTriggerHander : InputHandler
{
    private PlayerCombatControl _resultHandler;
    private Animator m_Animator;
    private Coroutine chargeCoroutine;
    private float currentChargeTime = 0f;
    private SkillData currentSkillData;
    private bool isCharging = false;
    private int skillIndex;
    public ChargeTriggerHander(GameObject owner) : base(owner)
    {
        _resultHandler=owner.GetComponent<PlayerCombatControl>();
        m_Animator=owner.GetComponent<Animator>();
    }

    public override InputHanderResult HandleInput(SkillData data, int skillInventoryIndex)
    {
        //蓄力的责任链处理脚本
        skillIndex = skillInventoryIndex;
        if (data.InputType == InputType.Charge)
        {
            //蓄力逻辑一直处于按下
            //开始蓄力的逻辑
            currentSkillData = data;

            // 开始蓄力协程
            if (chargeCoroutine == null)
            {
                chargeCoroutine =  CoroutineHelper.MainInstance.StartCoroutine(ChargeSkill());
            }

            return new InputHanderResult(data,0f,true);

        }
        return _nextHandler?.HandleInput(data, skillInventoryIndex);
    }


    //协程开启蓄力的协程
    IEnumerator ChargeSkill()
    {
        isCharging = true;
        currentChargeTime = 0f;

        // 播放蓄力动画
        // 获取到
        ResourcesAssetFactory factory = GameBaseFactory.GetAssetFactory() as ResourcesAssetFactory;
        CharacterComboSO so = factory.LoadSO(currentSkillData.ComboSOPath) as CharacterComboSO;

        m_Animator.Play(so.TryGetOneComboAction(0));
        

        while (true)
        {
            // 更新蓄力时间
            currentChargeTime += Time.deltaTime;
            Debug.Log("蓄力时间为" + currentChargeTime);


            // 检查是否达到最小蓄力时间
            bool hasMinCharge = currentChargeTime >= 0.2f;

            // 检查是否释放按键
            // 检查是否释放按键 - 根据 skillIndex 决定检测哪个按键
            bool skillReleased = false;
            if (skillIndex == 0)
            {
                skillReleased = GameInputManager.MainInstance.SkillQRelease;
            }
            else if (skillIndex == 1)
            {
                skillReleased = GameInputManager.MainInstance.SkillERelease;
            }
            if (skillReleased)
            {
                Debug.Log("停止蓄力" + "发动攻击");
                // 创建蓄力结果
                var result = new InputHanderResult(currentSkillData,skillIndex);
                result.IsCharged = hasMinCharge;
                result.ChargeTime = currentChargeTime;


                // 返回结果给处理器
                _resultHandler?.TriggerSkill(result);

                // 重置状态
                isCharging = false;
                chargeCoroutine = null;
                yield break;
            }

            yield return null;
        }
    }
}
