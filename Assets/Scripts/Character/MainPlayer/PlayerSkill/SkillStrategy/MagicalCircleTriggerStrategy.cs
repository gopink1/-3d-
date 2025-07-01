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
        //����ħ�����ȼ��ܵ�Ч��
        //��ȡԤ����
        if (result.SkillData.GetType() != typeof(MagicalCircleSkillData))
        {
            Debug.LogError("��������"+result.SkillData.Name + "���������ͷ��ϴ���");
        }
        MagicalCircleSkillData skillData = result.SkillData as MagicalCircleSkillData;
        ResourcesAssetFactory f1 = GameBaseFactory.GetAssetFactory() as ResourcesAssetFactory;
        //����comboSO
        CharacterComboSO data = f1.LoadSO(skillData.ComboSOPath) as CharacterComboSO;
        animator.Play(data.TryGetOneComboAction(0));

        //����Ԥ����
        GameObject pre = f1.LoadModel(skillData.EffectPrePath);

        //��ȡ���˺���ʼ��
        GameObject obj = GameObject.Instantiate(pre);
        Vector3 relativeOffset = animator.transform.TransformDirection(skillData.TargetPos);
        obj.GetComponent<MagicalCircleControl>().Init(animator.gameObject.transform.position + relativeOffset,skillData.Duration,skillData.EffectCount,skillData.EffectRate);

    }

}
