using MyTools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHash
{
    //public static readonly string ChangeCharacterVerticalVelocity = "ChangeCharacterVerticalVelocity";
    //���ó���״̬
    public static readonly string ChangeSceneState = "ChangeSceneState";

    //��ɫ��Ծ
    public static readonly string ChangeCharacterVerticalVelocity = "ChangeCharacterVerticalVelocity";

    //��ɫ����
    public static readonly string EnableGravity = "EnableGravity";

    public static readonly string OnCharacterHit = "OnCharacterHit";

    public static readonly string OnCharacterHealing = "OnCharacterHealing";

    public static readonly string OnCameraViewLock = "OnCameraViewLock";

    //���ƻ�ȡ���¼�
    public static readonly string ADDAttrCard = "ADDAttrCard";
    public static readonly string EquipSkill = "EquipSkill";
    public static readonly string ADDItemCard = "ADDItemCard";

    //��Ʒ����Ӱ������¼�
    public static readonly string DamageTaken = "DamageTaken";
    public static readonly string AttrChange = "AttrChange";

    //��Ʒ����ʾ������
    public static readonly string StartStockControl = "StartStockControl";
    public static readonly string FloatingText = "FloatingText";

    //�ؿ�ϵͳ���ܵ���
    public static readonly string BossKill = "BossKill";



    //����ui
    public static readonly string UpdateStageTimer = "UpdateStageTimer";
    public static readonly string UpdateStageText = "UpdateStageText";
    public static readonly string UpdateStageKill = "UpdateStageKill";
}
public class GameEventManager : SingletonNonMono<GameEventManager>
{
    private interface IEventHelp { }//�ӿ�ͬ��

    private class EventHelp : IEventHelp
    {
        private event Action action;

        //���캯��
        public EventHelp(Action action)
        {
            this.action = action;
        }

        public void AddCall(Action action)
        {
            this.action += action;
        }

        public void InvokeCall()
        {
            this.action?.Invoke();
        }
        public void RemoveCall(Action action)
        {
            this.action -= action;
        }

    }

    private class EventHelp<T> : IEventHelp
    {
        private event Action<T> action;

        //���캯��
        public EventHelp(Action<T> action)
        {
            this.action = action;
        }

        public void AddCall(Action<T> action)
        {
            this.action += action;
        }

        public void InvokeCall(T value)
        {
            this.action?.Invoke(value);
        }
        public void RemoveCall(Action<T> action)
        {
            this.action -= action;
        }

    }
    private class EventHelp<T1, T2> : IEventHelp
    {
        private event Action<T1, T2> action;

        //���캯��
        public EventHelp(Action<T1, T2> action)
        {
            this.action = action;
        }

        public void AddCall(Action<T1, T2> action)
        {
            this.action += action;
        }

        public void InvokeCall(T1 value1, T2 value2)
        {
            this.action?.Invoke(value1, value2);
        }
        public void RemoveCall(Action<T1, T2> action)
        {
            this.action -= action;
        }

    }
    private class EventHelp<T1, T2, T3> : IEventHelp
    {
        private event System.Action<T1, T2, T3> action;

        //���캯��
        public EventHelp(Action<T1, T2, T3> action)
        {
            this.action = action;
        }

        public void AddCall(Action<T1, T2, T3> action)
        {
            this.action += action;
        }

        public void InvokeCall(T1 value1, T2 value2, T3 Value3)
        {
            this.action?.Invoke(value1, value2, Value3);
        }
        public void RemoveCall(Action<T1, T2, T3> action)
        {
            this.action -= action;
        }

    }
    private class EventHelp<T1, T2, T3, T4> : IEventHelp
    {
        private event System.Action<T1, T2, T3, T4> action;

        //���캯��
        public EventHelp(Action<T1, T2, T3, T4> action)
        {
            this.action = action;
        }

        public void AddCall(Action<T1, T2, T3, T4> action)
        {
            this.action += action;
        }

        public void InvokeCall(T1 value1, T2 value2, T3 Value3, T4 Value4)
        {
            this.action?.Invoke(value1, value2, Value3, Value4);
        }
        public void RemoveCall(Action<T1, T2, T3, T4> action)
        {
            this.action -= action;
        }

    }
    private class EventHelp<T1, T2, T3, T4, T5> : IEventHelp
    {
        private event System.Action<T1, T2, T3, T4, T5> action;

        //���캯��
        public EventHelp(Action<T1, T2, T3, T4, T5> action)
        {
            this.action = action;
        }

        public void AddCall(Action<T1, T2, T3, T4, T5> action)
        {
            this.action += action;
        }

        public void InvokeCall(T1 value1, T2 value2, T3 Value3, T4 Value4, T5 Value5)
        {
            this.action?.Invoke(value1, value2, Value3, Value4, Value5);
        }
        public void RemoveCall(Action<T1, T2, T3, T4, T5> action)
        {
            this.action -= action;
        }

    }

    private Dictionary<string, IEventHelp> dic = new Dictionary<string, IEventHelp>();


    /// <summary>
    /// ���ʱ�����
    /// </summary>
    /// <param name="action"></param>
    public void AddEventListening(string eventName, Action action)
    {
        if (dic.TryGetValue(eventName, out IEventHelp e))
        {
            //����ҵ��¼���������Ҫ����ӣ�ֻ��Ҫ�������ȡ��
            (e as EventHelp)?.AddCall(action);
        }
        else
        {
            dic.Add(eventName, new EventHelp(action));
            //Debug.Log("�����" + eventName +"���¼�");
        }
    }
    public void AddEventListening<T>(string eventName, Action<T> action)
    {
        if (dic.TryGetValue(eventName, out IEventHelp e))
        {
            //����ҵ��¼���������Ҫ����ӣ�ֻ��Ҫ�������ȡ��
            (e as EventHelp<T>)?.AddCall(action);

        }
        else
        {
            dic.Add(eventName, new EventHelp<T>(action));
            Debug.Log("�����" + eventName +"���¼�");
        }
    }
    public void AddEventListening<T1, T2>(string eventName, Action<T1, T2> action)
    {
        if (dic.TryGetValue(eventName, out IEventHelp e))
        {
            (e as EventHelp<T1, T2>)?.AddCall(action);
        }
        else
        {
            dic.Add(eventName, new EventHelp<T1, T2>(action));
        }
    }
    public void AddEventListening<T1, T2, T3>(string eventName, System.Action<T1, T2, T3> action)
    {
        if (dic.TryGetValue(eventName, out IEventHelp e))
        {
            (e as EventHelp<T1, T2, T3>)?.AddCall(action);
        }
        else
        {
            dic.Add(eventName, new EventHelp<T1, T2, T3>(action));
        }
    }
    public void AddEventListening<T1, T2, T3, T4>(string eventName, System.Action<T1, T2, T3, T4> action)
    {
        if (dic.TryGetValue(eventName, out IEventHelp e))
        {
            (e as EventHelp<T1, T2, T3, T4>)?.AddCall(action);
        }
        else
        {
            dic.Add(eventName, new EventHelp<T1, T2, T3, T4>(action));
        }
    }

    public void AddEventListening<T1, T2, T3, T4, T5>(string eventName, System.Action<T1, T2, T3, T4, T5> action)
    {
        if (dic.TryGetValue(eventName, out IEventHelp e))
        {
            (e as EventHelp<T1, T2, T3, T4, T5>)?.AddCall(action);
        }
        else
        {
            dic.Add(eventName, new EventHelp<T1, T2, T3, T4, T5>(action));
        }
    }


    public void CallEvent(string eventName)
    {
        if (dic.TryGetValue(eventName, out IEventHelp e))
        {
            (e as EventHelp)?.InvokeCall();
        }
        else
        {
            Debug.Log("û������Ϊ"+ "#"+eventName+ "#"+ "���¼�");
        }
    }
    public void CallEvent<T>(string eventName, T value)
    {
        if (dic.TryGetValue(eventName, out IEventHelp e))
        {
            (e as EventHelp<T>)?.InvokeCall(value);
        }
        else
        {
            Debug.Log("û������Ϊ"+ "#"+eventName+ "#"+ "���¼�");
        }
    }
    public void CallEvent<T1, T2>(string eventName, T1 value1, T2 value2)
    {
        if (dic.TryGetValue(eventName, out IEventHelp e))
        {
            (e as EventHelp<T1, T2>)?.InvokeCall(value1, value2);
        }
        else
        {
            Debug.Log("û������Ϊ"+ "#"+eventName+ "#"+ "���¼�");
        }
    }

    public void CallEvent<T1, T2, T3>(string eventName, T1 value1, T2 value2, T3 Value3)
    {
        if (dic.TryGetValue(eventName, out IEventHelp e))
        {
            (e as EventHelp<T1, T2, T3>)?.InvokeCall(value1, value2, Value3);
        }
        else
        {
            Debug.Log("û������Ϊ"+ "#"+eventName+ "#"+ "���¼�");
        }
    }

    public void CallEvent<T1, T2, T3, T4>(string eventName, T1 value1, T2 value2, T3 Value3, T4 Value4)
    {
        if (dic.TryGetValue(eventName, out IEventHelp e))
        {
            (e as EventHelp<T1, T2, T3, T4>)?.InvokeCall(value1, value2, Value3, Value4);
        }
        else
        {
            Debug.Log("û������Ϊ"+ "#"+eventName+ "#"+ "���¼�");
        }
    }
    public void CallEvent<T1, T2, T3, T4, T5>(string eventName, T1 value1, T2 value2, T3 Value3, T4 Value4, T5 Value5)
    {
        if (dic.TryGetValue(eventName, out IEventHelp e))
        {
            (e as EventHelp<T1, T2, T3, T4, T5>)?.InvokeCall(value1, value2, Value3, Value4, Value5);
        }
        else
        {
            Debug.Log("û������Ϊ"+ "#"+eventName+ "#"+ "���¼�");
        }
    }


    public void RemoveEvent(string eventName, Action action)
    {
        if (dic.TryGetValue(eventName, out IEventHelp e))
        {
            (e as EventHelp)?.RemoveCall(action);
        }
        else
        {
            Debug.Log("û������Ϊ"+ "#"+eventName+ "#"+ "���¼�");
        }
    }
    public void RemoveEvent<T>(string eventName, Action<T> action)
    {
        if (dic.TryGetValue(eventName, out IEventHelp e))
        {
            (e as EventHelp<T>)?.RemoveCall(action);
        }
        else
        {
            Debug.Log("û������Ϊ"+ "#"+eventName+ "#"+ "���¼�");
        }
    }
    public void RemoveEvent<T1, T2>(string eventName, Action<T1, T2> action)
    {
        if (dic.TryGetValue(eventName, out IEventHelp e))
        {
            (e as EventHelp<T1, T2>)?.RemoveCall(action);
        }
        else
        {
            Debug.Log("û������Ϊ"+ "#"+eventName+ "#"+ "���¼�");
        }
    }
    public void RemoveEvent<T1, T2, T3>(string eventName, Action<T1, T2, T3> action)
    {
        if (dic.TryGetValue(eventName, out IEventHelp e))
        {
            (e as EventHelp<T1, T2, T3>)?.RemoveCall(action);
        }
        else
        {
            Debug.Log("û������Ϊ"+ "#"+eventName+ "#"+ "���¼�");
        }
    }
    public void RemoveEvent<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> action)
    {
        if (dic.TryGetValue(eventName, out IEventHelp e))
        {
            (e as EventHelp<T1, T2, T3, T4>)?.RemoveCall(action);
        }
        else
        {
            Debug.Log("û������Ϊ"+ "#"+eventName+ "#"+ "���¼�");
        }
    }
    public void RemoveEvent<T1, T2, T3, T4, T5>(string eventName, System.Action<T1, T2, T3, T4, T5> action)
    {
        if (dic.TryGetValue(eventName, out IEventHelp e))
        {
            (e as EventHelp<T1, T2, T3, T4, T5>)?.RemoveCall(action);
        }
        else
        {
            Debug.Log("û������Ϊ"+ "#"+eventName+ "#"+ "���¼�");
        }
    }

}
