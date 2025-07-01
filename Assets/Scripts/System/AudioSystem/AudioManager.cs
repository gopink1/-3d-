using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("SoundEmitters pool")]
    [SerializeField] private SoundEmitterFactory _factory = default;//����SoundEmitter�Ĺ���
    [SerializeField] private SoundEmitterPool _pool = default;//�����Ԥ��SoundEmitter
    [SerializeField] private int _initialSize = 10;//�����Ԥ�ص�����

    [Header("Listening on channels")]
    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play SFXs")]
    [SerializeField] private AudioCueEventChannelSO _SFXEventChannel = default;//������Ϸ��Ч�¼�ϵͳ
    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play Music")]
    [SerializeField] private AudioCueEventChannelSO _musicEventChannel = default;//������Ϸ�����¼�ϵͳ

    [Header("Audio control")]
    [SerializeField] private AudioMixer audioMixer = default;//��Ƶ����������ڿ�����Ƶ��������������
    [Range(0f, 1f)]
    [SerializeField] private float _masterVolume = 1f;//������
    [Range(0f, 1f)]
    [SerializeField] private float _musicVolume = 1f;//��������
    [Range(0f, 1f)]
    [SerializeField] private float _sfxVolume = 1f;//��Ч����

    private SoundEmitterVault _soundEmitterVault;//��Ч�������洢�⣬���ڹ�����Ч�������ļ�ֵ��
    private SoundEmitter _musicSoundEmitter;//��ǰ���ڲ������ֵ���Ч������

    private void Awake()
    {
        _pool = new SoundEmitterPool();
        _factory = _pool.GetFactory();

        //TODO: Get the initial volume levels from the settings
        _soundEmitterVault = new SoundEmitterVault();
        _pool.SetParent(this.transform);
        _pool.PrewarmPool(_initialSize);
    }
    private void OnEnable()
    {
        _SFXEventChannel.OnAudioCuePlayRequested += PlayAudioCue;
        _SFXEventChannel.OnAudioCueStopRequested += StopAudioCue;
        _SFXEventChannel.OnAudioCueFinishRequested += FinishAudioCue;

        _musicEventChannel.OnAudioCuePlayRequested += PlayMusicTrack;
        _musicEventChannel.OnAudioCueStopRequested += StopMusic;
    }

    private void OnDestroy()
    {
        _SFXEventChannel.OnAudioCuePlayRequested -= PlayAudioCue;
        _SFXEventChannel.OnAudioCueStopRequested -= StopAudioCue;
        _SFXEventChannel.OnAudioCueFinishRequested -= FinishAudioCue;

        _musicEventChannel.OnAudioCuePlayRequested -= PlayMusicTrack;
        _musicEventChannel.OnAudioCueStopRequested -= StopMusic;
    }
    /// <summary>
	/// This is only used in the Editor, to debug volumes.
	/// It is called when any of the variables is changed, and will directly change the value of the volumes on the AudioMixer.
	/// </summary>
	void OnValidate()
    {
        if (Application.isPlaying)
        {
            SetGroupVolume("MasterVolume", _masterVolume);
            SetGroupVolume("MusicVolume", _musicVolume);
            SetGroupVolume("SFXVolume", _sfxVolume);
        }
    }

    public void SetGroupVolume(string parameterName, float normalizedVolume)
    {
        bool volumeSet = audioMixer.SetFloat(parameterName, NormalizedToMixerValue(normalizedVolume));
        if (!volumeSet)
            Debug.LogError("The AudioMixer parameter was not found");
    }

    public float GetGroupVolume(string parameterName)
    {
        if (audioMixer.GetFloat(parameterName, out float rawVolume))
        {
            return MixerValueToNormalized(rawVolume);
        }
        else
        {
            Debug.LogError("The AudioMixer parameter was not found");
            return 0f;
        }
    }
    // Both MixerValueNormalized and NormalizedToMixerValue functions are used for easier transformations
    /// when using UI sliders normalized format
    private float MixerValueToNormalized(float mixerValue)
    {
        // We're assuming the range [-80dB to 0dB] becomes [0 to 1]
        return 1f + (mixerValue / 80f);
    }
    private float NormalizedToMixerValue(float normalizedValue)
    {
        // We're assuming the range [0 to 1] becomes [-80dB to 0dB]
        // This doesn't allow values over 0dB
        return (normalizedValue - 1f) * 80f;
    }
    private AudioCueKey PlayMusicTrack(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace)
    {
        float fadeDuration = 2f;//����ʵ��
        float startTime = 0f;//��ʼ��ʱ��

        if (_musicSoundEmitter != null && _musicSoundEmitter.IsPlaying())//����ǰ���������ڲ��ŵ�ʱ��ſ��Խ��ж����ֵĸı�
        {
            AudioClip songToPlay = audioCue.GetClips()[0];//��ȡ����Ҫ�л������ֵ�AudioCueSO�е�����clip
            if (_musicSoundEmitter.GetClip() == songToPlay)//�����ǰ����������Ҫ�л�������ͬ����Ҫ�����л�ֱ���˳���ǰ�ķ���
                return AudioCueKey.Invalid;

            //Music is already playing, need to fade it out
            startTime = _musicSoundEmitter.FadeMusicOut(fadeDuration);//��ʼ���ŵ�ʵ��Ϊ��һ�����ֽ�����
        }

        _musicSoundEmitter = _pool.Request();//��������һ������������
        _musicSoundEmitter.FadeMusicIn(audioCue.GetClips()[0], audioConfiguration, 1f, startTime);//�����AudioCue�л�ȡ������
        _musicSoundEmitter.OnSoundFinishedPlaying += StopMusicEmitter;//��ӻص�������ǰ���������������в�����Ϻ󣬹رյ�ǰ������������

        return AudioCueKey.Invalid; //No need to return a valid key for music//��Ϊ�������ֲ���Ҫ���з���valid
    }

    /// <summary>
    /// ֹͣ����
    /// </summary>
    /// <param name="key">û�����ֲ��Ų���Ҫkey</param>
    /// <returns></returns>
    private bool StopMusic(AudioCueKey key)
    {
        if (_musicSoundEmitter != null && _musicSoundEmitter.IsPlaying())
        {
            _musicSoundEmitter.Stop();
            return true;
        }
        else
            return false;
    }
    /// <summary>
	/// Plays an AudioCue by requesting the appropriate number of SoundEmitters from the pool.
	/// </summary>
	public AudioCueKey PlayAudioCue(AudioCueSO audioCue, AudioConfigurationSO settings, Vector3 position = default)
    {
        AudioClip[] clipsToPlay = audioCue.GetClips();//��ȡ��Ҫ��������Ƭ��
        SoundEmitter[] soundEmitterArray = new SoundEmitter[clipsToPlay.Length];//��ȡ����Ƭ������������������

        int nOfClips = clipsToPlay.Length;//��������
        for (int i = 0; i < nOfClips; i++)//��������
        {
            soundEmitterArray[i] = _pool.Request();//�������ػ�ȡ����
            if (soundEmitterArray[i] != null)
            {
                soundEmitterArray[i].PlayAudioClip(clipsToPlay[i], settings, audioCue.looping, position);//ʹ���������������Ŷ�ӦƬ��
                if (!audioCue.looping)
                    soundEmitterArray[i].OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;//���������Ҫѭ�����ŵ�����Ƭ�ξ��ٲ��Ž�������л���
            }
        }
        return _soundEmitterVault.Add(audioCue, soundEmitterArray);//���뵽��Ч�������洢��
    }
    /// <summary>
    /// ���ĳ������ѭ�����ŵ�����
    /// </summary>
    /// <param name="audioCueKey"></param>
    /// <returns></returns>
    public bool FinishAudioCue(AudioCueKey audioCueKey)
    {
        bool isFound = _soundEmitterVault.Get(audioCueKey, out SoundEmitter[] soundEmitters);//����Ч�������Ĵ洢���в�ѯ

        if (isFound)//�����ѯ��
        {
            for (int i = 0; i < soundEmitters.Length; i++)//����������
            {
                soundEmitters[i].Finish();//��������������ɺ���
                soundEmitters[i].OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;//�����ص�
            }
        }
        else
        {
            Debug.LogWarning("Finishing an AudioCue was requested, but the AudioCue was not found.");
        }

        return isFound;
    }
    /// <summary>
    /// ֹͣ��Ч������
    /// </summary>
    /// <param name="audioCueKey"></param>
    /// <returns></returns>
    public bool StopAudioCue(AudioCueKey audioCueKey)
    {
        bool isFound = _soundEmitterVault.Get(audioCueKey, out SoundEmitter[] soundEmitters);//�Ƿ��ѯ��

        if (isFound)
        {
            for (int i = 0; i < soundEmitters.Length; i++)
            {
                StopAndCleanEmitter(soundEmitters[i]);//ֹͣ���������Ч
            }

            _soundEmitterVault.Remove(audioCueKey);
        }

        return isFound;
    }
    /// <summary>
    /// ����Ч��ɲ��ź����
    /// </summary>
    /// <param name="soundEmitter"></param>
    private void OnSoundEmitterFinishedPlaying(SoundEmitter soundEmitter)
    {
        StopAndCleanEmitter(soundEmitter);
    }
    //���
    private void StopAndCleanEmitter(SoundEmitter soundEmitter)
    {
        if (!soundEmitter.IsLooping())
            soundEmitter.OnSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;//��ѭ������Ч�ȽӴ��ص���

        soundEmitter.Stop();//ֹͣ����
        _pool.Return(soundEmitter);//���ض����

        //TODO: is the above enough?
        //_soundEmitterVault.Remove(audioCueKey); is never called if StopAndClean is called after a Finish event
        //How is the key removed from the vault?
    }

    /// <summary>
    /// ֹͣ���ַ������Ľ���
    /// ������ѭ�����ֽ���ʱ�����
    /// </summary>
    /// <param name="soundEmitter"></param>
    private void StopMusicEmitter(SoundEmitter soundEmitter)
    {
        soundEmitter.OnSoundFinishedPlaying -= StopMusicEmitter;
        _pool.Return(soundEmitter);
    }
}
