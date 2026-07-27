using UnityEngine;

public static class GameOption
{
    private const string KEY_AUTO_ATTACK = "Option.AutoAttack";
    private const string KEY_HELPER_SHOWN = "Option.HelperShown";

    private static bool _isAutoAttack = false;
    private static bool _isHelperShown = false;
    private static bool _isLoaded = false;

    public static bool IsAutoAttack
    {
        get
        {
            Load();
            return _isAutoAttack;
        }
    }

    public static void SetAutoAttack(bool isOn)
    {
        _isAutoAttack = isOn;
        _isLoaded = true;

        if (isOn == true)
        {
            PlayerPrefs.SetInt(KEY_AUTO_ATTACK, 1);
        }

        else
        {
            PlayerPrefs.SetInt(KEY_AUTO_ATTACK, 0);
        }

        PlayerPrefs.Save();
    }

    public static bool IsHelperShown
    {
        get
        {
            Load();
            return _isHelperShown;
        }
    }

    public static void SetHelperShown(bool isShown)
    {
        _isHelperShown = isShown;
        _isLoaded = true;

        if (isShown == true)
        {
            PlayerPrefs.SetInt(KEY_HELPER_SHOWN, 1);
        }

        else
        {
            PlayerPrefs.SetInt(KEY_HELPER_SHOWN, 0);
        }

        PlayerPrefs.Save();
    }

    private static void Load()
    {
        if (_isLoaded == true)
        {
            return;
        }

        _isAutoAttack = (PlayerPrefs.GetInt(KEY_AUTO_ATTACK, 0) == 1);
        _isHelperShown = (PlayerPrefs.GetInt(KEY_HELPER_SHOWN, 0) == 1);
        _isLoaded = true;
    }
}
