using UnityEngine;

public class CursorHandler
{
    private ParticleSystem _clickEffect;

    public void Init()
    {
        _clickEffect = Object.Instantiate(Utils.ResourcesLoad<ParticleSystem>("Cursor/ClickEffect"));
        _clickEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void OnLeftClickEffect(Vector3 position)
    {
        if (_clickEffect == null)
        {
            Debug.LogError("ClickEffect is not initialized.");
            return;
        }

        _clickEffect.transform.position = position;

        _clickEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _clickEffect.Play(true);
    }
}
