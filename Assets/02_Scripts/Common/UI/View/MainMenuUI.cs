using UnityEngine;

public class MainMenuUI : BaseUI
{
    [SerializeField] private UIButton _inventoryButton;
    [SerializeField] private UIButton _characterButton;
    [SerializeField] private UIButton _startBattleButton;
    [SerializeField] private UIButton _optionButton;
    [SerializeField] private UIButton _farmButton;


    [Header("Animation")]
    [SerializeField] private UISlideAnimation _slideAnimation;

    protected override void OnInit()
    {
        _inventoryButton.BindOnClickButtonEvent(OnClickInventory);
        _characterButton.BindOnClickButtonEvent(OnClickCharacter);
        _startBattleButton.BindOnClickButtonEvent(OnClickStartBattleButton);
        _optionButton.BindOnClickButtonEvent(OnClickOption);
        _farmButton.BindOnClickButtonEvent(OnClickFarm);
    }

    protected override void OnOpened()
    {
        if (_slideAnimation == null)
        {
            return;
        }

        _slideAnimation.SetHidden();
        _slideAnimation.SlideIn();
    }

    // 임시: 나중에 Model 소유처 정해지면 이동
    private InventoryModel _inventoryModelTest = new InventoryModel();
    private PetInventoryModel _petInventoryModelTest = new PetInventoryModel();

    private void OnClickInventory()
    {
        if (_inventoryModelTest.ItemList.Count == 0)
        {
            CreateTestItemList();
        }

        GameManager.UI.OpenInventoryUI(_inventoryModelTest);
    }

    private void CreateTestItemList()
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

    private void OnClickCharacter()
    {
        Debug.Log("캐릭터 창 열기");
    }

    private void OnClickStartBattleButton()
    {
        if (_petInventoryModelTest.PetList.Count == 0)
        {
            CreateTestPetList();
        }

        GameManager.UI.OpenPetInventoryUI(_petInventoryModelTest);
    }

    private void CreateTestPetList()
    {
        long uniqueId = 1;

        foreach (var petData in GameManager.DataTable.PetDataTable.Values)
        {
            var pet = new PetSlotModel();
            pet.PetUniqueId = uniqueId;
            pet.PetDataId = petData.Id;
            pet.Level = 1;
            _petInventoryModelTest.AddPet(pet);
            uniqueId++;
        }
    }

    private void OnClickFarm()
    {
        LocationNavigator locationNavigator = Object.FindFirstObjectByType<LocationNavigator>();

        if (locationNavigator == null )
        {
            Debug.LogWarning("LocationNavigator를 찾을 수 없습니다.");
            return;
        }

        locationNavigator.ToggleMonsterFarm();

        string buttonText = locationNavigator.IsInMonsterFarm ? "마을 복귀" : "팜";

        _farmButton.ChangeText(buttonText);
    }

    private void OnClickOption()
    {
        Debug.Log("설정 열기");
        // GameManager.UI.OpenUI<OptionUI>(UIType.OptionUI);
    }
}
