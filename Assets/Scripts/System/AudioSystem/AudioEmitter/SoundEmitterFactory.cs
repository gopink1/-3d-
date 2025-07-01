using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitterFactory
{

    private SoundEmitter prefab;                                    //����������Ԥ����

    private string emitterPath = "Audio/SoundEmitter/SoundEmitter"; //����������Ԥ����·��

    /// <summary>
    /// ��������������
    /// </summary>
    /// <returns></returns>
    public SoundEmitter CreateSoundEmitter()
    {
        SoundEmitter emitter = null;
        //����Resources�ļ��������ж�Ԥ�����ʼ��
        GameObject pre =  GameBaseFactory.GetAssetFactory().LoadModel(emitterPath);
        emitter =  GameObject.Instantiate(pre).GetComponent<SoundEmitter>();
        return emitter;
    }
}
