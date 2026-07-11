using UnityEngine;

public partial class UIManager
{
    public T OpenUI<T>(UIType uiType) where T : BaseUI
    {
        T ui = GetCreatUI<T>(uiType);

        if(ui == null)
        {
            return null;
        }

        if (_activeUI.Contains(uiType) == false)
        {
            ui.ActiveTrue();
            _activeUI.Add(uiType);
        }
        return ui;
    }

    public void CloseUI(UIType uiType)
    {

        if (_activeUI.Contains(uiType) == false)
        {
            return;
        }
   
        if (_createdUIDic.TryGetValue(uiType, out BaseUI ui) == false)
        {
            _activeUI.Remove(uiType);
            return;
        }

        ui.ActiveFalse();
        _activeUI.Remove(uiType);
    }

    public bool IsActiveUI(UIType uiType)
    {
        return _activeUI.Contains(uiType);
    }
}
