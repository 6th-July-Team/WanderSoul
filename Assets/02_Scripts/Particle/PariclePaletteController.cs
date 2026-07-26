using System;
using UnityEngine;

public enum EffectColorType
{
    Red,
    Blue,
    Brown,
    Yellow,
    Purple
}

[Serializable]
public class EffectPalette
{
    public EffectColorType Type;

    [Range(0f, 360f)]
    public float TargetHue;

    [Range(0f, 2f)]
    public float SaturationScale = 1f;

    [Range(0f, 2f)]
    public float ValueScale = 1f;

    [Range(0f, 1f)]
    public float MinSaturation = 0.1f;
}

public class PariclePaletteController : MonoBehaviour
{
    [SerializeField] private EffectPalette[] _palettes;

    private ParticleSystem[] _particleSystems;
    private ParticleSystem.MinMaxGradient[] _originalStartColors;

    private void Awake()
    {
        _particleSystems = GetComponentsInChildren<ParticleSystem>(true);
        _originalStartColors = new ParticleSystem.MinMaxGradient[_particleSystems.Length];

        for (int i = 0; i < _particleSystems.Length; i++)
        {
            _originalStartColors[i] = _particleSystems[i].main.startColor;
        }
    }

    public void ApplyPalette(EffectColorType colorType)
    {
        EffectPalette palette = Array.Find(_palettes, palette => palette.Type == colorType);

        for (int i = 0; i < _particleSystems.Length; i++)
        {
            ParticleSystem.MainModule main = _particleSystems[i].main;

            main.startColor = ChangeGradient(_originalStartColors[i], palette);
        }
    }

    private ParticleSystem.MinMaxGradient ChangeGradient(ParticleSystem.MinMaxGradient source, EffectPalette palette)
    {
        switch (source.mode)
        {
            case ParticleSystemGradientMode.Color:
                return new ParticleSystem.MinMaxGradient(ChangeColor(source.color, palette));

            case ParticleSystemGradientMode.TwoColors:
                return new ParticleSystem.MinMaxGradient(ChangeColor(source.colorMin, palette)
                    , ChangeColor(source.colorMax, palette));

            case ParticleSystemGradientMode.Gradient:
                return new ParticleSystem.MinMaxGradient(ChangeGradient(source.gradient, palette));

            case ParticleSystemGradientMode.TwoGradients:
                return new ParticleSystem.MinMaxGradient(ChangeGradient(source.gradientMin, palette)
                    , ChangeGradient(source.gradientMax, palette)
                );

            case ParticleSystemGradientMode.RandomColor:
                {
                    var result = new ParticleSystem.MinMaxGradient(ChangeGradient(source.gradient, palette));

                    result.mode = ParticleSystemGradientMode.RandomColor;
                    return result;
                }

            default:
                return source;
        }
    }

    private Gradient ChangeGradient(Gradient source, EffectPalette palette)
    {
        GradientColorKey[] colorKeys = source.colorKeys;
        GradientAlphaKey[] alphaKeys = source.alphaKeys;

        for (int i = 0; i < colorKeys.Length; i++)
        {
            colorKeys[i].color = ChangeColor(colorKeys[i].color, palette);
        }

        Gradient result = new Gradient();
        result.SetKeys(colorKeys, alphaKeys);
        result.mode = source.mode;

        return result;
    }

    private Color ChangeColor(Color source, EffectPalette palette)
    {
        Color.RGBToHSV(source, out _, out float saturation, out float value);

        float targetHue = palette.TargetHue / 360f;

        saturation = Mathf.Clamp01(saturation * palette.SaturationScale);

        saturation = Mathf.Max(saturation, palette.MinSaturation);

        value *= palette.ValueScale;

        Color result = Color.HSVToRGB(targetHue, saturation, value, true);

        result.a = source.a;

        return result;
    }
}