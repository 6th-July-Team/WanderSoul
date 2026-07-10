using UnityEngine;

public class UIGameFlowTest : MonoBehaviour
{
    void Start()
    {
        ShowTitle();
    }

    private void ShowTitle()
    {
        GameManager.UI.OpenUI<TitleUI>(UIType.TitleUI);
    }

    public void StartGame()
    {
        GameManager.UI.CloseUI(UIType.TitleUI);
        ShowGameHud();
    }

    private void ShowGameHud()
    {
        GameManager.UI.OpenUI<MainMenuUI>(UIType.MainMenuUI);
    }
}
