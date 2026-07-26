
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SoundManager
{
    private AudioSource SFXSourcePlayer;
    private AudioSource BGMSourcePlayer;

    private readonly Dictionary<string, CancellationTokenSource> _repeatingSfxCtsDic = new();

    private readonly Dictionary<string, float> _repeatingSfxIntervalDic = new();

    private float _bgmVolume = 1f;
    private float _sfxVolume = 1f;

    public void Init(GameObject gameManager)
    {
        SFXSourcePlayer = Utils.GetOrAddComponentInChild<AudioSource>(gameManager, "SFXSourcePlayer");
        BGMSourcePlayer = Utils.GetOrAddComponentInChild<AudioSource>(gameManager, "BGMSourcePlayer");

        ApplyVolume();
    }

    public void PlaySFX(string soundDataId)
    {
        

        //Utils.LoadAndPlayAudioClip(SFXSourcePlayer, data.Address, data.IsLoop, data.Volume);
    }

    public void PlayBGM(string soundDataId)
    {
        

        //Utils.LoadAndPlayAudioClip(BGMSourcePlayer, data.Address, data.IsLoop, data.Volume);
    }
    public float GetBGMVolume()
    {
        return _bgmVolume;
    }

    public float GetSFXVolume()
    {
        return _sfxVolume;
    }

    public void SetBGMVolume(float volumeRatio)
    {
        _bgmVolume = Mathf.Clamp01(volumeRatio);

        if (null == BGMSourcePlayer) return;

        BGMSourcePlayer.volume = _bgmVolume;
    }

    public void SetSFXVolume(float volumeRatio)
    {
        _sfxVolume = Mathf.Clamp01(volumeRatio);

        if (null == SFXSourcePlayer) return;

        SFXSourcePlayer.volume = _sfxVolume;
    }

    private void ApplyVolume()
    {
        SetBGMVolume(_bgmVolume);
        SetSFXVolume(_sfxVolume);
    }

    public void StopBGM()
    {
        if (null == BGMSourcePlayer) return;

        BGMSourcePlayer.Stop();
    }
}