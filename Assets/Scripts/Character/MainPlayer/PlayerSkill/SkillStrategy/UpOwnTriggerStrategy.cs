using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpOwnTriggerStrategy : ISkillTriggerStrategy
{
    private Animator animator;
    public UpOwnTriggerStrategy(Animator animator)
    {
        this.animator = animator;
    }

    public void triggerSkill(InputHanderResult result)
    {
        //触发魔法剑等技能的效果
        //获取预制体
        if (result.SkillData.GetType() != typeof(BuffSkillData))
        {
            Debug.LogError("触发技能"+result.SkillData.Name + "所触发类型符合错误");
        }
        BuffSkillData skillData = result.SkillData as BuffSkillData;
        ResourcesAssetFactory f1 = GameBaseFactory.GetAssetFactory() as ResourcesAssetFactory;
        //生成comboSO
        CharacterComboSO data = f1.LoadSO(skillData.ComboSOPath) as CharacterComboSO;
        animator.Play(data.TryGetOneComboAction(0));
        //生成预制体
        GameObject pre = f1.LoadModel(skillData.EffectPrePath);
        //实例化
        GameObject obj = GameObject.Instantiate(pre);
        FollowingSkillControl control = obj.GetComponent<FollowingSkillControl>();
        control.InitSkill(GameBase.MainInstance.GetMainPlayer().transform,skillData.Duration,skillData.EffectCount,skillData.Effect);
    }
}
