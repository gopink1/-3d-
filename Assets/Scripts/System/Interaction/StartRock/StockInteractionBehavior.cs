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
        //石头交互后石头
        //1.石头消失
        //2.开始游戏
        //石头消失
        //_animator.Play("StockOut");
        //使用dotween进行是石头移动到看不到的位置
        DoHide();
        //2.开启关卡
        //激活关卡
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
