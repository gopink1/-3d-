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
        //���������߼�����������
        //��ȡԤ����
        if (result.SkillData.GetType() != typeof(ChargeComboSkillData))
        {
            Debug.LogError("��������"+result.SkillData.Name + "���������ͷ��ϴ���");
        }
        ChargeComboSkillData skillData = result.SkillData as ChargeComboSkillData;
        ResourcesAssetFactory f1 = GameBaseFactory.GetAssetFactory() as ResourcesAssetFactory;
        //����comboSO
        CharacterComboSO data = f1.LoadSO(skillData.ComboSOPath) as CharacterComboSO;
        animator.Play(data.TryGetOneComboAction(2));
        //������ЧԤ����
        GameObject pre = f1.LoadModel(skillData.EffectPrePath);
        //��ʼ����Ч
        GameObject obj = GameObject.Instantiate(pre);

        obj.GetComponent<TriggerSkillControl>().Init(skillData.RelativeOffset,animator.gameObject.transform,result.ChargeTime,skillData.MaxChargeTime);
    }
}
