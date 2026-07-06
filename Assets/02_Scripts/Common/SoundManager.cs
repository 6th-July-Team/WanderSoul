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

    //public void PlaySFX(string soundDataId)
    //{
    //    SoundData data = GameManager.DataTable.GetSoundData(soundDataId);
    //    if (null == data)
    //    {
    //        Debug.LogError($"사운드 데이터를 찾을 수 없습니다: {soundDataId}");
    //        return;
    //    }

    //    Utils.LoadAndPlayAudioClip(SFXSourcePlayer, data.Name, data.IsLoop, data.Volume);
    //}

    //public void PlayBGM(string soundDataId)
    //{
    //    SoundData data = GameManager.DataTable.GetSoundData(soundDataId);
    //    if (null == data)
    //    {
    //        Debug.LogError($"사운드 데이터를 찾을 수 없습니다: {soundDataId}");
    //        return;
    //    }

    //    Utils.LoadAndPlayAudioClip(BGMSourcePlayer, data.Name, data.IsLoop, data.Volume);
    //}

    //public void StopBGM()
    //{
    //    if (null == BGMSourcePlayer) return;

    //    BGMSourcePlayer.Stop();
    //}

    //public void SetBGMVolume(string soundDataId, float volumeRatio)
    //{
    //    if (null == BGMSourcePlayer) return;

    //    SoundData data = GameManager.DataTable.GetSoundData(soundDataId);
    //    float baseVolume = data != null ? data.Volume : 1f;

    //    BGMSourcePlayer.volume = Mathf.Clamp01(volumeRatio) * baseVolume;
    //}

    //public void SetBGMPitch(float pitch)
    //{
    //    if (null == BGMSourcePlayer) return;

    //    BGMSourcePlayer.pitch = Mathf.Clamp(pitch, 0f, 2f);
    //}
}