using UnityEngine;

public class TitleUI : BaseUI
{
    [SerializeField] private UIButton _startButton;
    [SerializeField] private UIButton _optionButton;
    [SerializeField] private UIButton _quitButton;

    protected override void OnInit()
    {
        _startButton.BindOnClickButtonEvent(OnClickStart);
        _optionButton.BindOnClickButtonEvent(OnClickOption);
        _quitButton.BindOnClickButtonEvent(OnClickQuit);
    }

    private void OnClickStart()
    {
        Debug.Log("게임 시작");
        GameManager.UI.CloseUI(UIType.TitleUI);

        GameManager.UI.OpenUI<MainMenuUI>(UIType.MainMenuUI);

        // 테스트용
        OpenResourceHudTest();
        OpenPartyHudTest();
        OpenVillageInfoHudTest();
        OpenSkillHudTest();
    }

    private void OnClickOption()
    {
        Debug.Log("설정 열기");
        // GameManager.UI.OpenUI<OptionUI>(UIType.OptionUI);
    }

    private void OnClickQuit()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }

    private void OpenResourceHudTest()
    {
        var model = new ResourceModel();
        model.Soul = 12413451;
        model.Money = 8520;

        var viewModel = new ResourceHudViewModel(model);
        var view = GameManager.UI.OpenUI<ResourceHudUIView>(UIType.ResourceHudUIView);
        if (view != null)
        {
            view.BindViewModel(viewModel);
        }
    }

    private void OpenPartyHudTest()
    {
        var model = new PartyMemberModel();
        model.Name = "피슬";
        model.MaxHp = 1000;
        model.CurrentHp = 800;

        var viewModel = new PartyHudViewModel(model);
        var view = GameManager.UI.OpenUI<PartyHudUIView>(UIType.PartyHudUIView);
        if (view != null)
        {
            view.BindViewModel(viewModel);
        }
    }

    private void OpenVillageInfoHudTest()
    {
        var model = new VillageModel();
        model.TownDataId = "town_lavendil";
        model.CurrentReputation = 50;

        var viewModel = new VillageInfoViewModel(model);
        var view = GameManager.UI.OpenUI<VillageInfoHudUIView>(UIType.VillageInfoHudUIView);
        if (view != null)
        {
            view.BindViewModel(viewModel);
        }
    }

    private void OpenSkillHudTest()
    {
        var view = GameManager.UI.OpenUI<SkillHudUIView>(UIType.SkillHudUIView);
        if (view == null)
        {
            Debug.LogWarning("스킬 HUD를 열 수 없습니다");
        }
    }
}