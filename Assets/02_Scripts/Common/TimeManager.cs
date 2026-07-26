using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class TimeManager
{
    public float GameDeltaTime
    {
        get
        {
            return IsPaused ? 0f : Time.deltaTime;
        }
    }

    public bool IsPaused
    {
        get
        {
            return _pauseCount > 0;
        }
    }

    private int _pauseCount = 0;

    public void Init()
    {
        _pauseCount = 0;
    }

    public void OnPause()
    {
        _pauseCount++;
    }

    public void OnResume()
    {
        _pauseCount = Mathf.Max(0, _pauseCount - 1);
    }

    public async UniTask<bool> WaitForGameSeconds(float duration, CancellationToken token = default)
    {
        float remainingTime = duration;

        while (remainingTime > 0f)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);

            if (token.IsCancellationRequested)
            {
                return false;
            }

            remainingTime -= GameDeltaTime;
        }

        return true;
    }
}
