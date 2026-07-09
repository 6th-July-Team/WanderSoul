using UnityEngine;

public class TimeManager
{
    public float GameDeltaTIme
    {
        get
        {
            return IsPaused ? 0f : Time.deltaTime;
        }
    }

    public bool IsPaused { get; private set; }

    public void Init()
    {
        IsPaused = false;
    }

    public void OnPause()
    {
        IsPaused = true;
    }

    public void OnResume()
    {
        IsPaused = false;
    }
}
