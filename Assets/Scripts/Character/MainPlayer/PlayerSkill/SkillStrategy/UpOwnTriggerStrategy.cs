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
        //����ħ�����ȼ��ܵ�Ч��
        //��ȡԤ����
        if (result.SkillData.GetType() != typeof(BuffSkillData))
        {
            Debug.LogError("��������"+result.SkillData.Name + "���������ͷ��ϴ���");
        }
        BuffSkillData skillData = result.SkillData as BuffSkillData;
        ResourcesAssetFactory f1 = GameBaseFactory.GetAssetFactory() as ResourcesAssetFactory;
        //����comboSO
        CharacterComboSO data = f1.LoadSO(skillData.ComboSOPath) as CharacterComboSO;
        animator.Play(data.TryGetOneComboAction(0));
        //����Ԥ����
        GameObject pre = f1.LoadModel(skillData.EffectPrePath);
        //ʵ����
        GameObject obj = GameObject.Instantiate(pre);
        FollowingSkillControl control = obj.GetComponent<FollowingSkillControl>();
        control.InitSkill(GameBase.MainInstance.GetMainPlayer().transform,skillData.Duration,skillData.EffectCount,skillData.Effect);
    }
}
