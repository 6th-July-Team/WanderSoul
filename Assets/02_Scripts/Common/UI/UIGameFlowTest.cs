using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIGameFlowTest : MonoBehaviour
{
    void Start()
    {
        ShowTitle();
    }

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            OpenLevelUpTest();
        }

        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            OpenLoadingTest();
        }
    }

    private void ShowTitle()
    {
        var titleUI = GameManager.UI.OpenUI<TitleUI>(UIType.TitleUI);

        if (titleUI != null)
        {
            titleUI.OnStartClicked += StartGame;
        }
    }

    public void StartGame()
    {
        GameManager.UI.CloseUI(UIType.TitleUI);
        ShowGameHud();
    }

    private void ShowGameHud()
    {
        GameManager.UI.OpenUI<MainMenuUI>(UIType.MainMenuUI);

        OpenResourceHudTest();
        OpenPartyHudTest();
        OpenVillageInfoHudTest();
        OpenSkillHudTest();
    }

    private void OpenResourceHudTest()
    {
        var model = new ResourceModel();
        model.Soul = 12413451;
        model.Money = 8520;

        GameManager.UI.OpenResourceHudUI(model);
    }

    private void OpenPartyHudTest()
    {
        var view = GameManager.UI.OpenPartyHudUI();
        if (view == null)
        {
            return;
        }

        view.SetWagon("마차", 1f);
        view.AddPet("펫1", 0.8f);
        view.AddPet("펫2", 0.5f);
        view.AddPet("펫3", 0.3f);

    }

    private void OpenVillageInfoHudTest()
    {
        var model = new VillageModel();
        model.TownDataId = "town_lavendil";
        model.CurrentReputation = 50;

        GameManager.UI.OpenVillageInfoHudUI(model);
    }

    private void OpenSkillHudTest()
    {
        GameManager.UI.OpenSkillHudUI();
    }

    private void OpenLevelUpTest()
    {
        var testIds = new List<string> { "옵션ID1", "옵션ID2", "옵션ID3" };
        GameManager.UI.OpenLevelUpUI(testIds);
    }

    private async void OpenLoadingTest()
    {
        var loading = GameManager.UI.OpenLoadingUI();
        if (loading == null)
        {
            return;
        }

        float elapsed = 0f;
        float duration = 3f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            loading.SetProgress(elapsed / duration);
            await Cysharp.Threading.Tasks.UniTask.Yield();
        }

        loading.SetProgress(1f);
        GameManager.UI.CloseUI(UIType.LoadingUIView);
    }
}
