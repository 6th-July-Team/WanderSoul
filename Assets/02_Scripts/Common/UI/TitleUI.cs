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



        //테스트용
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

    // 설정 버튼 - 설정 UI 열기
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
}