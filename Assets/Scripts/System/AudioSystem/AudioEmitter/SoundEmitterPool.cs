using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �����������Ķ����
/// </summary>
public class SoundEmitterPool
{
    private SoundEmitterFactory soundEmitterFactory = new SoundEmitterFactory();            //�������������������Ĺ���

    public SoundEmitterFactory GetFactory() { return soundEmitterFactory; }
    public SoundEmitterFactory SoundEmitterFactory { get { return soundEmitterFactory; } }  //�ⲿ��ȡ

    private GameObject PoolObj = null;

    private Stack<SoundEmitter> Available = new Stack<SoundEmitter>();  //Ԥ�صĶ����
    private bool HasBeenPrewarmed { get; set; }


    /// <summary>
    /// ����ض�������
    /// </summary>
    public void SetParent(Transform parent)
    {
        //��ʼ��Pool���½�һ����Ϸ����
        //�����лẬ��û�н��м���Ķ����
        PoolObj = parent.gameObject;
    }

    private SoundEmitter Create()
    {
        return SoundEmitterFactory.CreateSoundEmitter();
    }
    public void PrewarmPool(int num)
    {
        if (HasBeenPrewarmed)
        {
            Debug.LogWarning($"Pool SoundEmitter has already been prewarmed.");
            return;
        }
        for (int i = 0; i < num; i++)
        {
            SoundEmitter sm = Create();
            sm.gameObject.transform.SetParent(PoolObj.transform);
            sm.gameObject.SetActive(false);
            Available.Push(sm);
        }
        HasBeenPrewarmed = true;
    }
    /// <summary>
    /// ���󷵻�����������
    /// </summary>
    /// <returns></returns>
    public virtual SoundEmitter Request()
    {
        SoundEmitter sm = Available.Count > 0 ? Available.Pop() : Create();
        sm.gameObject.SetActive(true);
        return sm;
    }
    /// <summary>
    /// �������������󷽷���������������
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public IEnumerable<SoundEmitter> Request(int num = 1)
    {
        List<SoundEmitter> members = new List<SoundEmitter>(num);
        for (int i = 0; i < num; i++)
        {
            members.Add(Request());
        }
        return members;
    }

    //�������������ض����
    public void Return(SoundEmitter member)
    {
        member.gameObject.SetActive(false);
        Available.Push(member);
    }

    public virtual void Return(IEnumerable<SoundEmitter> members)
    {
        foreach (SoundEmitter member in members)
        {
            Return(member);
        }
    }

    public void OnClear()
    {
        Available.Clear();
        HasBeenPrewarmed = false;
    }
}
