using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalCircleTriggerStrategy : ISkillTriggerStrategy
{

    private Animator animator;
    public MagicalCircleTriggerStrategy(Animator animator)
    {
        this.animator = animator;
    }
    public void triggerSkill(InputHanderResult result)
    {
        //触发魔法剑等技能的效果
        //获取预制体
        if (result.SkillData.GetType() != typeof(MagicalCircleSkillData))
        {
            Debug.LogError("触发技能"+result.SkillData.Name + "所触发类型符合错误");
        }
        MagicalCircleSkillData skillData = result.SkillData as MagicalCircleSkillData;
        ResourcesAssetFactory f1 = GameBaseFactory.GetAssetFactory() as ResourcesAssetFactory;
        //生成comboSO
        CharacterComboSO data = f1.LoadSO(skillData.ComboSOPath) as CharacterComboSO;
        animator.Play(data.TryGetOneComboAction(0));

        //生成预制体
        GameObject pre = f1.LoadModel(skillData.EffectPrePath);

        //读取的伤害初始化
        GameObject obj = GameObject.Instantiate(pre);
        Vector3 relativeOffset = animator.transform.TransformDirection(skillData.TargetPos);
        obj.GetComponent<MagicalCircleControl>().Init(animator.gameObject.transform.position + relativeOffset,skillData.Duration,skillData.EffectCount,skillData.EffectRate);

    }

}
