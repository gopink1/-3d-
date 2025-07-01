using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockInteractionBehavior : InteractionBehaviorBase
{
    private Vector3 startPos;
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        startPos = transform.position;
        GameEventManager.MainInstance.AddEventListening<bool>(EventHash.StartStockControl, ControlStock);

    }
    private void OnDestroy()
    {
        GameEventManager.MainInstance.RemoveEvent<bool>(EventHash.StartStockControl, ControlStock);
    }
    protected override void Interaction()
    {
        //ʯͷ������ʯͷ
        //1.ʯͷ��ʧ
        //2.��ʼ��Ϸ
        //ʯͷ��ʧ
        //_animator.Play("StockOut");
        //ʹ��dotween������ʯͷ�ƶ�����������λ��
        DoHide();
        //2.�����ؿ�
        //����ؿ�
        GameBase.MainInstance.ApplyStage(true);
    }
    public void ControlStock(bool isShow)
    {
        if(isShow)
        {
            DoShow();
        }
        else
        {
            DoHide() ;
        }
    }
    public void DoHide()
    {
        gameObject.transform.DOMove(new Vector3(transform.position.x, transform.position.y - 3f, transform.position.z), 1.5f);
    }
    public void DoShow()
    {
        gameObject.transform.DOMove(startPos, 1.5f);
    }
}
