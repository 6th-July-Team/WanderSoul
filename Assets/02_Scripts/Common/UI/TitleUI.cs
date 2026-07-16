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

        GameManager.UI.OpenResourceHudUI(model);
    }

    private void OpenPartyHudTest()
    {
        var model = new PartyMemberModel();
        model.Name = "피슬";
        model.MaxHp = 1000;
        model.CurrentHp = 800;

        GameManager.UI.OpenPartyHudUI(model);
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
}