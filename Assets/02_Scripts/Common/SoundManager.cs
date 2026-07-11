using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SoundManager
{
    private AudioSource SFXSourcePlayer;
    private AudioSource BGMSourcePlayer;

    private readonly Dictionary<string, CancellationTokenSource> _repeatingSfxCtsDic = new();

    private readonly Dictionary<string, float> _repeatingSfxIntervalDic = new();

    public void Init(GameObject gameManager)
    {
        SFXSourcePlayer = Utils.GetOrAddComponentInChild<AudioSource>(gameManager, "SFXSourcePlayer");
        BGMSourcePlayer = Utils.GetOrAddComponentInChild<AudioSource>(gameManager, "BGMSourcePlayer");
    }

    public void PlaySFX(string soundDataId)
    {
        

        //Utils.LoadAndPlayAudioClip(SFXSourcePlayer, data.Address, data.IsLoop, data.Volume);
    }

    public void PlayBGM(string soundDataId)
    {
        

        //Utils.LoadAndPlayAudioClip(BGMSourcePlayer, data.Address, data.IsLoop, data.Volume);
    }
    public void SetBGMVolume(float volumeRatio)
    {
        
    }

    public void SetSFXVolume(float volumeRatio)
    {

    }

    public void StopBGM()
    {
        if (null == BGMSourcePlayer) return;

        BGMSourcePlayer.Stop();
    }
}