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
        ApplyTimeScale();
    }

    public void OnPause()
    {
        _pauseCount++;
        ApplyTimeScale();
    }

    public void OnResume()
    {
        _pauseCount = Mathf.Max(0, _pauseCount - 1);
        ApplyTimeScale();
    }

    private void ApplyTimeScale()
    {
        if (IsPaused == true)
        {
            Time.timeScale = 0f;
            return;
        }

        Time.timeScale = 1f;
    }
}
