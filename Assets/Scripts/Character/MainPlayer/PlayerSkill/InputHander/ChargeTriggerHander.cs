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
        //����������������ű�
        skillIndex = skillInventoryIndex;
        if (data.InputType == InputType.Charge)
        {
            //�����߼�һֱ���ڰ���
            //��ʼ�������߼�
            currentSkillData = data;

            // ��ʼ����Э��
            if (chargeCoroutine == null)
            {
                chargeCoroutine =  CoroutineHelper.MainInstance.StartCoroutine(ChargeSkill());
            }

            return new InputHanderResult(data,0f,true);

        }
        return _nextHandler?.HandleInput(data, skillInventoryIndex);
    }


    //Э�̿���������Э��
    IEnumerator ChargeSkill()
    {
        isCharging = true;
        currentChargeTime = 0f;

        // ������������
        // ��ȡ��
        ResourcesAssetFactory factory = GameBaseFactory.GetAssetFactory() as ResourcesAssetFactory;
        CharacterComboSO so = factory.LoadSO(currentSkillData.ComboSOPath) as CharacterComboSO;

        m_Animator.Play(so.TryGetOneComboAction(0));
        

        while (true)
        {
            // ��������ʱ��
            currentChargeTime += Time.deltaTime;
            Debug.Log("����ʱ��Ϊ" + currentChargeTime);


            // ����Ƿ�ﵽ��С����ʱ��
            bool hasMinCharge = currentChargeTime >= 0.2f;

            // ����Ƿ��ͷŰ���
            // ����Ƿ��ͷŰ��� - ���� skillIndex ��������ĸ�����
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
                Debug.Log("ֹͣ����" + "��������");
                // �����������
                var result = new InputHanderResult(currentSkillData,skillIndex);
                result.IsCharged = hasMinCharge;
                result.ChargeTime = currentChargeTime;


                // ���ؽ����������
                _resultHandler?.TriggerSkill(result);

                // ����״̬
                isCharging = false;
                chargeCoroutine = null;
                yield break;
            }

            yield return null;
        }
    }
}
