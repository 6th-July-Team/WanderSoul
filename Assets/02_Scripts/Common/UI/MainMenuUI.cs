using UnityEngine;

public class MainMenuUI : BaseUI<MainMenuUI>
{
    [SerializeField] private UIButton _inventoryButton;
    [SerializeField] private UIButton _characterButton;
    [SerializeField] private UIButton _farmButton;
    [SerializeField] private UIButton _optionButton;

    protected override void OnInit()
    {
        _inventoryButton.BindOnClickButtonEvent(OnClickInventory);
        _characterButton.BindOnClickButtonEvent(OnClickCharacter);
        _farmButton.BindOnClickButtonEvent(OnClickFarm);
        _optionButton.BindOnClickButtonEvent(OnClickOption);
    }

    private void OnClickInventory()
    {
        Debug.Log("인벤토리 열기");
    }

    private void OnClickCharacter()
    {
        Debug.Log("캐릭터 창 열기");
    }

    private void OnClickFarm()
    {
        Debug.Log("농장 열기");
    }

    private void OnClickOption()
    {
        Debug.Log("설정 열기");
        // GameManager.UI.OpenUI<OptionUI>(UIType.OptionUI);
    }
}
