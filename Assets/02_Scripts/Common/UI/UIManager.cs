using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonBehaviour<UIManager>
{
    [SerializeField] private Canvas _backgroundCanvas;
    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private Canvas _contentCanvas;
    [SerializeField] private Canvas _popupCanvas;
    [SerializeField] private Canvas _frontCanvas;

    private Dictionary<UIType, BaseUI> _createdUIDic = new Dictionary<UIType, BaseUI>();

    private HashSet<UIType> _activeUI = new HashSet<UIType>();

    protected override void Init()
    {
        base.Init();
    }
}