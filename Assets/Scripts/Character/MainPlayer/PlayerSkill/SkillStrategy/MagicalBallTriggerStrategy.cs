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
        //����ħ�����ȼ��ܵ�Ч��
        //��ȡԤ����
        if(result.SkillData.GetType() != typeof(MagicalBallSkillData))
        {
            Debug.LogError("��������"+result.SkillData.Name + "���������ͷ��ϴ���");
        }
        MagicalBallSkillData skillData = result.SkillData as MagicalBallSkillData;
        ResourcesAssetFactory f1 = GameBaseFactory.GetAssetFactory() as ResourcesAssetFactory;

        //����comboSO
        CharacterComboSO data = f1.LoadSO(skillData.ComboSOPath) as CharacterComboSO;
        animator.Play(data.TryGetOneComboAction(0));

        //����Ԥ����
        GameObject pre = f1.LoadModel(skillData.EffectPrePath);

        //��ʼ������Ч��
        GameObject obj = GameObject.Instantiate(pre);
        //���ݾ�����ٶ��趨Ŀ��
        //�趨Ϊ��ǰ��Ϊ�����켣

        Vector3 target = animator.gameObject.transform.position + animator.gameObject.transform.forward * skillData.SkillRange + animator.gameObject.transform.up * 0.9f;
        obj.transform.rotation = animator.gameObject.transform.rotation;
        obj.transform.position = animator.gameObject.transform.position;
        obj.GetComponent<IceFireControl>().Launch(target,skillData.SkillRange);

    }
}
