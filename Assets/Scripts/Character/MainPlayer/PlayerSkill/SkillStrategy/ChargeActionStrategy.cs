using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class ChargeActionStrategy : ISkillTriggerStrategy
{
    private Animator animator;
    public ChargeActionStrategy(Animator animator)
    {
        this.animator = animator;
    }
    public void triggerSkill(InputHanderResult result)
    {
        //蓄力触发逻辑，触发的是
        //获取预制体
        if (result.SkillData.GetType() != typeof(ChargeComboSkillData))
        {
            Debug.LogError("触发技能"+result.SkillData.Name + "所触发类型符合错误");
        }
        ChargeComboSkillData skillData = result.SkillData as ChargeComboSkillData;
        ResourcesAssetFactory f1 = GameBaseFactory.GetAssetFactory() as ResourcesAssetFactory;
        //生成comboSO
        CharacterComboSO data = f1.LoadSO(skillData.ComboSOPath) as CharacterComboSO;
        animator.Play(data.TryGetOneComboAction(2));
        //生成特效预制体
        GameObject pre = f1.LoadModel(skillData.EffectPrePath);
        //初始化特效
        GameObject obj = GameObject.Instantiate(pre);

        obj.GetComponent<TriggerSkillControl>().Init(skillData.RelativeOffset,animator.gameObject.transform,result.ChargeTime,skillData.MaxChargeTime);
    }
}
