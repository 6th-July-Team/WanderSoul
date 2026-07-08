using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private Canvas _backgroundRoot;
    private Canvas _mainRoot;
    private Canvas _contentRoot;
    private Canvas _popupRoot;
    private Canvas _veryFrontRoot;


    private Dictionary<UIType, BaseUI> _createdUIDic = new Dictionary<UIType, BaseUI>();

    private HashSet<UIType> _activeUI = new HashSet<UIType>();

    private void Init()
    {
        var uiHandler = GameObject.Instantiate(Resources.Load<GameObject>("UICanvas"));

        if (uiHandler.TryGetComponent(out UIManagerHandler handler))
        {
            _backgroundRoot = handler.BackgroundRoot;
            _mainRoot = handler.MainRoot;
            _contentRoot = handler.ContentRoot;
            _popupRoot = handler.PopupRoot;
            _veryFrontRoot = handler.VeryFrontRoot;
        }

        else
        {
            Debug.LogWarning("UI Root Canvas Prefab에 UIMgrHandler 컴포넌트가 없습니다.");
        }
    }

    private T GetCreatUI<T>(UIType uiType) where T : BaseUI<T>
    {
        string path = GetUIPath(uiType);

        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError($"{uiType}의 경로를 찾을 수 없습니다.");
            return null;
        }

        Canvas rootCanvas = GetRootCansvas(GetUIRootType(uiType));

        if(rootCanvas == null)
        {
            Debug.LogError($"{uiType}의 캔버스를 찾을 수 없습니다.");
            return null;
        }

        GameObject prefab = Utils.ResourcesLoad<GameObject>(path);

        if (prefab == null)
        {
            Debug.LogError($"프리팹 로드 실패 : {path}");
            return null;
        }

        GameObject uiObject = GameObject.Instantiate(prefab,rootCanvas.transform);

        if(uiObject == null)
        {
            Debug.LogError($"UI 생성 실패 : {path}");
            return null;
        }

        T newUI = uiObject.GetComponent<T>();

        if (newUI == null)
        {
            Debug.LogError($"{uiObject.name}에 {typeof(T)} 컴포넌트가 없습니다.");
            return null;
        }

        newUI.Init();
        newUI.ActiveFalse();
        _createdUIDic.Add(uiType, newUI);

        return newUI;
    }

    private string GetUIPath(UIType uiType)
    {
        UIRootType rootType = GetUIRootType(uiType);
        return $"Prefabs/UI/{rootType}/{uiType}";
    }

    private UIRootType GetUIRootType(UIType uiType)
    {
        switch (uiType)
        {
            case UIType.TitleUI:
                return UIRootType.Main;
            case UIType.InventoryUI:
                return UIRootType.Main;
            case UIType.PetInfoUI:
                return UIRootType.Content;
            case UIType.TopUI:
                return UIRootType.Main;
            case UIType.MainUI:
                return UIRootType.Main;
            case UIType.PlayerHudUI:
                return UIRootType.Main;
            case UIType.QuestHudUI:
                return UIRootType.Main;
            case UIType.PopupUI:
                return UIRootType.Popup;
            case UIType.MapUI:
                return UIRootType.Content;
            case UIType.DialogueUI:
                return UIRootType.Popup;
            default:
                Debug.LogError($"{uiType}에 지정된 UIRootType이 없습니다.");
                return UIRootType.Main;
        }
    }

    private Canvas GetRootCansvas(UIRootType rootType)
    {
        switch (rootType)
        {
            case UIRootType.Background:
                return _backgroundRoot;
            case UIRootType.Main:
                return _mainRoot;
            case UIRootType.Content:
                return _contentRoot;
            case UIRootType.Popup:
                return _popupRoot;
            case UIRootType.Front:
                return _veryFrontRoot;
            default:
                Debug.LogError($"{rootType}에 맞는 캔버스가 없습니다.");
                return null;
        }
    }
}
