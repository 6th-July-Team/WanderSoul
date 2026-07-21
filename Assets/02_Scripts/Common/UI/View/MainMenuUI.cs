using UnityEngine;

public class MainMenuUI : BaseUI
{
    [SerializeField] private UIButton _inventoryButton;
    [SerializeField] private UIButton _characterButton;
    [SerializeField] private UIButton _farmButton;
    [SerializeField] private UIButton _optionButton;

    [Header("Animation")]
    [SerializeField] private UISlideAnimation _slideAnimation;

    protected override void OnInit()
    {
        _inventoryButton.BindOnClickButtonEvent(OnClickInventory);
        _characterButton.BindOnClickButtonEvent(OnClickCharacter);
        _farmButton.BindOnClickButtonEvent(OnClickFarm);
        _optionButton.BindOnClickButtonEvent(OnClickOption);
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

    private void OnClickFarm()
    {
        if (_petInventoryModelTest.PetList.Count == 0)
        {
            CreateTestPetList();
        }

        GameManager.UI.OpenPetInventoryUI(_petInventoryModelTest);
    }

    private void CreateTestPetList()
    {
        string[] testPetDataIdArray = new string[]
        {
        "pet_fire_001", "pet_water_002", "pet_earth_003", "pet_air_004",
        "pet_fire_001", "pet_water_002", "pet_earth_003", "pet_air_004",
        "pet_fire_001"
        };

        long uniqueId = 1;

        foreach (string petDataId in testPetDataIdArray)
        {
            var pet = new PetSlotModel();
            pet.PetUniqueId = uniqueId;
            pet.PetDataId = petDataId;
            pet.Level = 1;

            _petInventoryModelTest.AddPet(pet);
            uniqueId++;
        }
    }

    private void OnClickOption()
    {
        Debug.Log("설정 열기");
        // GameManager.UI.OpenUI<OptionUI>(UIType.OptionUI);
    }
}
