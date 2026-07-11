#if UNITY_EDITOR

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class FullScreenMode : MonoBehaviour, InputSystem_Default.IEditorActions
{
    InputSystem_Default inputSystem;
    [SerializeField]
    [Tooltip("게임 시작시 자동으로 전체화면으로 전환하는 옵션\n오류가 없음을 확신할 때 사용하세요")]
    bool makeFullscreenAtStart = false;

    private void Awake()
    {
        inputSystem = new ();
        inputSystem.Editor.SetCallbacks(this);
        inputSystem.Editor.Enable();
    }

    void Start() { if (makeFullscreenAtStart) { FullscreenGameView.Toggle(); } }

    public void OnFullScreen(InputAction.CallbackContext value)
    {
        if (value.started)
            FullscreenGameView.Toggle();
    }
}


public static class FullscreenGameView
{
    static readonly Type GameViewType = Type.GetType("UnityEditor.GameView,UnityEditor");
    static readonly PropertyInfo ShowToolbarProperty = GameViewType.GetProperty("showToolbar", BindingFlags.Instance | BindingFlags.NonPublic);
    static readonly object False = false; // Only box once. This is a matter of principle.

    static EditorWindow instance;

    // Exit fullscreen when re-compiling game during Game session (to fix bug where can't leave fullscreen)
    static FullscreenGameView() { AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload; }
    private static void OnBeforeAssemblyReload() { if (instance != null) { instance.Close(); instance = null; } }

    [MenuItem("Window/General/Game (Fullscreen) %#&2", priority = 2)]
    public static void Toggle()
    {
        if (GameViewType == null)
        {
            Debug.LogError("GameView type not found.");
            return;
        }

        if (ShowToolbarProperty == null)
        {
            Debug.LogWarning("GameView.showToolbar property not found.");
        }

        if (instance != null)
        {
            instance.Close();
            instance = null;
        }
        else
        {
            instance = (EditorWindow)ScriptableObject.CreateInstance(GameViewType);

            ShowToolbarProperty?.SetValue(instance, False);

            var desktopResolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
            var fullscreenRect = new Rect(Vector2.zero, desktopResolution);
            instance.ShowPopup();
            instance.position = fullscreenRect;
            instance.Focus();
        }
    }
}

#endif