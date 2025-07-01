using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 声音发射器的对象池
/// </summary>
public class SoundEmitterPool
{
    private SoundEmitterFactory soundEmitterFactory = new SoundEmitterFactory();            //用于生成声音发射器的工厂

    public SoundEmitterFactory GetFactory() { return soundEmitterFactory; }
    public SoundEmitterFactory SoundEmitterFactory { get { return soundEmitterFactory; } }  //外部获取

    private GameObject PoolObj = null;

    private Stack<SoundEmitter> Available = new Stack<SoundEmitter>();  //预载的对象池
    private bool HasBeenPrewarmed { get; set; }


    /// <summary>
    /// 对象池对象父物体
    /// </summary>
    public void SetParent(Transform parent)
    {
        //初始化Pool，新建一个游戏物体
        //物体中会含有没有进行激活的对象的
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
    /// 请求返回声音发射器
    /// </summary>
    /// <returns></returns>
    public virtual SoundEmitter Request()
    {
        SoundEmitter sm = Available.Count > 0 ? Available.Pop() : Create();
        sm.gameObject.SetActive(true);
        return sm;
    }
    /// <summary>
    /// 声音发射器请求方法返回声音发射器
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

    //声音发射器返回对象池
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
