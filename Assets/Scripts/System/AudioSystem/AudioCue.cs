using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCue : MonoBehaviour
{
    [Header("Sound definition")]
    [SerializeField] private AudioCueSO _audioCue = default;//声音资源
    [SerializeField] private bool _playOnStart = false;//是否需要挂载到的物体激活时候就播放音频

    [Header("Configuration")]
    [SerializeField] private AudioCueEventChannelSO _audioCueEventChannel = default;//事件系统，是与AudioManager通讯的中枢
    [SerializeField] private AudioConfigurationSO _audioConfiguration = default;//声音配置信息，AudioSources的配置

    private AudioCueKey controlKey = AudioCueKey.Invalid;

    private void Start()
    {
        if (_playOnStart)
            StartCoroutine(PlayDelayed());
    }

    private void OnDisable()
    {
        _playOnStart = false;
    }

    /// <summary>
    /// 延迟播放音频
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayDelayed()
    {
        //等待1s，确保AudioManager准备好播放
        yield return new WaitForSeconds(1f);

        //判断是否需要进行开始就播放
        if (_playOnStart)
            PlayAudioCue();
    }
    /// <summary>
    /// 呼叫AudioManager播放对象配置好的音频
    /// </summary>
    public void PlayAudioCue()
    {
        controlKey = _audioCueEventChannel.RaisePlayEvent(_audioCue, _audioConfiguration, transform.position);
    }
    /// <summary>
    /// 停止当前对象的音频播放
    /// </summary>
    public void StopAudioCue()
    {
        if (controlKey != AudioCueKey.Invalid)
        {
            if (!_audioCueEventChannel.RaiseStopEvent(controlKey))
            {
                controlKey = AudioCueKey.Invalid;
            }
        }
    }
    /// <summary>
    /// 当前对象音频播放完毕
    /// </summary>
    public void FinishAudioCue()
    {
        if (controlKey != AudioCueKey.Invalid)
        {
            if (!_audioCueEventChannel.RaiseFinishEvent(controlKey))
            {
                controlKey = AudioCueKey.Invalid;
            }
        }
    }
}
