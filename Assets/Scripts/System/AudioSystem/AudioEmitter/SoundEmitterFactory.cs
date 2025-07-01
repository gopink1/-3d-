using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitterFactory
{

    private SoundEmitter prefab;                                    //声音发射器预制体

    private string emitterPath = "Audio/SoundEmitter/SoundEmitter"; //声音发射器预制体路径

    /// <summary>
    /// 创建声音发射器
    /// </summary>
    /// <returns></returns>
    public SoundEmitter CreateSoundEmitter()
    {
        SoundEmitter emitter = null;
        //根据Resources文件夹来进行对预制体初始化
        GameObject pre =  GameBaseFactory.GetAssetFactory().LoadModel(emitterPath);
        emitter =  GameObject.Instantiate(pre).GetComponent<SoundEmitter>();
        return emitter;
    }
}
