using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCue : MonoBehaviour
{
    [Header("Sound definition")]
    [SerializeField] private AudioCueSO _audioCue = default;//������Դ
    [SerializeField] private bool _playOnStart = false;//�Ƿ���Ҫ���ص������弤��ʱ��Ͳ�����Ƶ

    [Header("Configuration")]
    [SerializeField] private AudioCueEventChannelSO _audioCueEventChannel = default;//�¼�ϵͳ������AudioManagerͨѶ������
    [SerializeField] private AudioConfigurationSO _audioConfiguration = default;//����������Ϣ��AudioSources������

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
    /// �ӳٲ�����Ƶ
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayDelayed()
    {
        //�ȴ�1s��ȷ��AudioManager׼���ò���
        yield return new WaitForSeconds(1f);

        //�ж��Ƿ���Ҫ���п�ʼ�Ͳ���
        if (_playOnStart)
            PlayAudioCue();
    }
    /// <summary>
    /// ����AudioManager���Ŷ������úõ���Ƶ
    /// </summary>
    public void PlayAudioCue()
    {
        controlKey = _audioCueEventChannel.RaisePlayEvent(_audioCue, _audioConfiguration, transform.position);
    }
    /// <summary>
    /// ֹͣ��ǰ�������Ƶ����
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
    /// ��ǰ������Ƶ�������
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
