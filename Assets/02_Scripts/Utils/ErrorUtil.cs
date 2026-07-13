using UnityEngine;

public static class Logger
{
    // 1. 추가적인 정보 표현 ex) 타임스탬프
    // 2. 츨시용 빌드를 위한 로그 제거

    #region Log
    [System.Diagnostics.Conditional("DEV_VER")]
    public static void Log(string msg)
    {
        Debug.LogFormat("[{0}] {1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), msg);
    }

    [System.Diagnostics.Conditional("DEV_VER")]
    public static void Log(Object context, string msg)
    {
        Debug.LogFormat(context, "[{0}] {1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), msg);
    }
    #endregion

    #region LogWarning
    [System.Diagnostics.Conditional("DEV_VER")]
    public static void LogWarning(string msg)
    {
        Debug.LogWarningFormat("[{0}] {1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), msg);
    }

    [System.Diagnostics.Conditional("DEV_VER")]
    public static void LogWarning(Object context, string msg)
    {
        Debug.LogWarningFormat(context, "[{0}] {1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), msg);
    }
    #endregion

    #region LogError
    public static void LogError(string msg)
    {
        Debug.LogErrorFormat("[{0}] {1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), msg);
    }

    public static void LogError(Object context, string msg)
    {
        Debug.LogErrorFormat(context, "[{0}] {1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), msg);
    }

    #endregion
}

public static class DataLog
{
    public static void EnumParseFailed<TEnum>(string className, string id, string targetJsonData) where TEnum : struct, System.Enum
    {
        Logger.LogError($"[{className} : {typeof(TEnum).Name}로의 형변환에 실패하였습니다!!] JSON Id : {id}, JSON Data : {targetJsonData}");
    }
}