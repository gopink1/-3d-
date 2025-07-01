using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using MyTools;
using System;

public class TimerManager : MyTools.Singleton<TimerManager>
{
    //����Timer����
    //��ʼ�յ�Timer����
    [SerializeField] private int initMaxTimerCount;

    private Queue<GameTimer> noWorkingTimers = new Queue<GameTimer>();
    private List<GameTimer> isWorkingTimers = new List<GameTimer>();

    private void Start()
    {
        InitGamerTimerManager();
        //��ʼ��
    }

    //��ʼ����Ϸʱ�������
    private void InitGamerTimerManager()
    {
        //��ʼ��һ�������Ŀռ�ʱ��
        for (int i = 0; i < initMaxTimerCount; i++)
        {
            GameTimer timer = new GameTimer();
            noWorkingTimers.Enqueue(timer);
        }
    }

    private void Update()
    {
        UpdateGameTimers();
    }

    public void TryEnableOneGameTimer(float time, Action action)
    {
        if (noWorkingTimers.Count == 0)
        {
            var newtimer = new GameTimer();
            noWorkingTimers.Enqueue(newtimer);
            var timer = noWorkingTimers.Dequeue();
            timer.StartTimer(time, action);
            isWorkingTimers.Add(timer);
        }
        else
        {
            var timer = noWorkingTimers.Dequeue();
            timer.StartTimer(time, action);
            isWorkingTimers.Add(timer);
        }

    }
    private void UpdateGameTimers()
    {
        if (isWorkingTimers.Count == 0) return;
        for (int i = 0; i < isWorkingTimers.Count; i++)
        {
            if (isWorkingTimers[i].GetTimerState == TimerState.Working)
            {
                isWorkingTimers[i].UpdateTimer();
            }
            else
            {
                //�Ӷ��� ����ʼ�� ɾ��
                noWorkingTimers.Enqueue(isWorkingTimers[i]);
                isWorkingTimers[i].ResetTimer();
                isWorkingTimers.Remove(isWorkingTimers[i]);
            }

        }
    }
}
