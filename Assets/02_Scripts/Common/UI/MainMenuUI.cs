using UnityEngine;

public class MainMenuUI : BaseUI
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
    // 임시: 나중에 PlayerModel 같은 곳으로 이동
    private InventoryModel _inventoryModelTest = new InventoryModel();

    private void OnClickInventory()
    {
        Debug.Log("인벤토리 열기");
        OpenInventoryTest();
    }

    private void OpenInventoryTest()
    {
        if (_inventoryModelTest.ItemList.Count == 0)
        {
            for (int i = 1; i <= 20; i++)
            {
                var item = new ItemSlotModel();
                item.ItemUniqueId = i;
                item.ItemDataId = $"item_{i}";
                item.StackCount = i;
                _inventoryModelTest.AddItem(item);
            }
        }

        var viewModel = new InventoryViewModel(_inventoryModelTest);
        var view = GameManager.UI.OpenUI<InventoryUIView>(UIType.InventoryUIView);
        if (view != null)
        {
            view.BindViewModel(viewModel);
        }
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
