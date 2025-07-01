using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalBallTriggerStrategy : ISkillTriggerStrategy
{
    private Animator animator;
    public MagicalBallTriggerStrategy(Animator animator)
    {
        this.animator = animator;
    }
    public void triggerSkill(InputHanderResult result)
    {
        //触发魔法剑等技能的效果
        //获取预制体
        if(result.SkillData.GetType() != typeof(MagicalBallSkillData))
        {
            Debug.LogError("触发技能"+result.SkillData.Name + "所触发类型符合错误");
        }
        MagicalBallSkillData skillData = result.SkillData as MagicalBallSkillData;
        ResourcesAssetFactory f1 = GameBaseFactory.GetAssetFactory() as ResourcesAssetFactory;

        //生成comboSO
        CharacterComboSO data = f1.LoadSO(skillData.ComboSOPath) as CharacterComboSO;
        animator.Play(data.TryGetOneComboAction(0));

        //生成预制体
        GameObject pre = f1.LoadModel(skillData.EffectPrePath);

        //初始化技能效果
        GameObject obj = GameObject.Instantiate(pre);
        //根据距离和速度设定目标
        //设定为正前方为弹道轨迹

        Vector3 target = animator.gameObject.transform.position + animator.gameObject.transform.forward * skillData.SkillRange + animator.gameObject.transform.up * 0.9f;
        obj.transform.rotation = animator.gameObject.transform.rotation;
        obj.transform.position = animator.gameObject.transform.position;
        obj.GetComponent<IceFireControl>().Launch(target,skillData.SkillRange);

    }
}
