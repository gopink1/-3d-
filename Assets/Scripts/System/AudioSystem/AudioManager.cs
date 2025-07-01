using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("SoundEmitters pool")]
    [SerializeField] private SoundEmitterFactory _factory = default;//生产SoundEmitter的工厂
    [SerializeField] private SoundEmitterPool _pool = default;//对象池预载SoundEmitter
    [SerializeField] private int _initialSize = 10;//对象池预载的数量

    [Header("Listening on channels")]
    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play SFXs")]
    [SerializeField] private AudioCueEventChannelSO _SFXEventChannel = default;//订阅游戏音效事件系统
    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play Music")]
    [SerializeField] private AudioCueEventChannelSO _musicEventChannel = default;//订阅游戏音乐事件系统

    [Header("Audio control")]
    [SerializeField] private AudioMixer audioMixer = default;//音频混合器，用于控制音频的音量、混音等
    [Range(0f, 1f)]
    [SerializeField] private float _masterVolume = 1f;//主音量
    [Range(0f, 1f)]
    [SerializeField] private float _musicVolume = 1f;//音乐音量
    [Range(0f, 1f)]
    [SerializeField] private float _sfxVolume = 1f;//音效音量

    private SoundEmitterVault _soundEmitterVault;//音效发射器存储库，用于管理音效发射器的键值对
    private SoundEmitter _musicSoundEmitter;//当前正在播放音乐的音效发射器

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
        float fadeDuration = 2f;//渐变实践
        float startTime = 0f;//开始计时器

        if (_musicSoundEmitter != null && _musicSoundEmitter.IsPlaying())//当当前的音乐正在播放的时候才可以进行对音乐的改变
        {
            AudioClip songToPlay = audioCue.GetClips()[0];//获取到需要切换的音乐的AudioCueSO中的音乐clip
            if (_musicSoundEmitter.GetClip() == songToPlay)//如果当前的音乐与需要切换音乐相同则不需要进行切换直接退出当前的方法
                return AudioCueKey.Invalid;

            //Music is already playing, need to fade it out
            startTime = _musicSoundEmitter.FadeMusicOut(fadeDuration);//开始播放的实践为上一个音乐渐出后
        }

        _musicSoundEmitter = _pool.Request();//请求对象池一个声音发送器
        _musicSoundEmitter.FadeMusicIn(audioCue.GetClips()[0], audioConfiguration, 1f, startTime);//渐入从AudioCue中获取的音乐
        _musicSoundEmitter.OnSoundFinishedPlaying += StopMusicEmitter;//添加回调，当当前的声音发射器进行播放完毕后，关闭当前的声音发射器

        return AudioCueKey.Invalid; //No need to return a valid key for music//因为播放音乐不需要进行返回valid
    }

    /// <summary>
    /// 停止音乐
    /// </summary>
    /// <param name="key">没用音乐播放不需要key</param>
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
        AudioClip[] clipsToPlay = audioCue.GetClips();//获取需要播放声音片段
        SoundEmitter[] soundEmitterArray = new SoundEmitter[clipsToPlay.Length];//获取声音片段数量的声音发射器

        int nOfClips = clipsToPlay.Length;//遍历次数
        for (int i = 0; i < nOfClips; i++)//遍历播放
        {
            soundEmitterArray[i] = _pool.Request();//请求对象池获取对象
            if (soundEmitterArray[i] != null)
            {
                soundEmitterArray[i].PlayAudioClip(clipsToPlay[i], settings, audioCue.looping, position);//使用声音发射器播放对应片段
                if (!audioCue.looping)
                    soundEmitterArray[i].OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;//如果不是需要循环播放的声音片段就再播放结束后进行回收
            }
        }
        return _soundEmitterVault.Add(audioCue, soundEmitterArray);//加入到音效发射器存储库
    }
    /// <summary>
    /// 完成某个真正循环播放的音乐
    /// </summary>
    /// <param name="audioCueKey"></param>
    /// <returns></returns>
    public bool FinishAudioCue(AudioCueKey audioCueKey)
    {
        bool isFound = _soundEmitterVault.Get(audioCueKey, out SoundEmitter[] soundEmitters);//再音效发射器的存储室中查询

        if (isFound)//如果查询到
        {
            for (int i = 0; i < soundEmitters.Length; i++)//遍历发射器
            {
                soundEmitters[i].Finish();//触发发射器的完成函数
                soundEmitters[i].OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;//结束回调
            }
        }
        else
        {
            Debug.LogWarning("Finishing an AudioCue was requested, but the AudioCue was not found.");
        }

        return isFound;
    }
    /// <summary>
    /// 停止音效并回收
    /// </summary>
    /// <param name="audioCueKey"></param>
    /// <returns></returns>
    public bool StopAudioCue(AudioCueKey audioCueKey)
    {
        bool isFound = _soundEmitterVault.Get(audioCueKey, out SoundEmitter[] soundEmitters);//是否查询到

        if (isFound)
        {
            for (int i = 0; i < soundEmitters.Length; i++)
            {
                StopAndCleanEmitter(soundEmitters[i]);//停止并且清除音效
            }

            _soundEmitterVault.Remove(audioCueKey);
        }

        return isFound;
    }
    /// <summary>
    /// 当音效完成播放后清除
    /// </summary>
    /// <param name="soundEmitter"></param>
    private void OnSoundEmitterFinishedPlaying(SoundEmitter soundEmitter)
    {
        StopAndCleanEmitter(soundEmitter);
    }
    //清除
    private void StopAndCleanEmitter(SoundEmitter soundEmitter)
    {
        if (!soundEmitter.IsLooping())
            soundEmitter.OnSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;//非循环的音效先接触回调绑定

        soundEmitter.Stop();//停止播放
        _pool.Return(soundEmitter);//返回对象池

        //TODO: is the above enough?
        //_soundEmitterVault.Remove(audioCueKey); is never called if StopAndClean is called after a Finish event
        //How is the key removed from the vault?
    }

    /// <summary>
    /// 停止音乐发射器的进行
    /// 即当非循环音乐结束时候调用
    /// </summary>
    /// <param name="soundEmitter"></param>
    private void StopMusicEmitter(SoundEmitter soundEmitter)
    {
        soundEmitter.OnSoundFinishedPlaying -= StopMusicEmitter;
        _pool.Return(soundEmitter);
    }
}
