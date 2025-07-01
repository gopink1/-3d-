using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingCircleControl : MagicalCircleControl
{
    [SerializeField] private float radius;
    [SerializeField] private LayerMask layers;



    protected float effectCd;
    private void Awake()
    {
        
    }
    protected override void Update()
    {
        if (effectCd == 0) effectCd = effectRate;
        base.Update();
        //���Ʒ�����
        //Ҫ�õ�ÿ��ظ���������

    }
    public override void UpdateWithTimer()
    {
        base.UpdateWithTimer();
        //ÿ֡���º�������ÿ֡�ظ��������жϺʹ���
        if(timer >  effectRate )
        {
            effectRate += effectCd;
            //������Ѫ
            //�ж�����Ƿ��ڷ�Χ��
            Collider[] colliders =  Physics.OverlapSphere(center, radius, layers);
            //�ж�
            // ����������ײ���Ķ���
            for (int i = 0; i < colliders.Length; i++)
            {
                Collider hitCollider = colliders[i];

                if (hitCollider.CompareTag("Player"))
                {
                    // �������Ҿʹ��������¼�
                    GameEventManager.MainInstance.CallEvent(
                        EventHash.OnCharacterHealing,
                        effectCount,
                        "",
                        hitCollider.transform
                    );
                }
            }
        }
    }

    ////��Χ������ķ��ػ���
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(c1, radius);

    //}
}
