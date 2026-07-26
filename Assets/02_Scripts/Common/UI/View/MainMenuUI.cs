using UnityEngine;

public class MainMenuUI : BaseUI
{
    [SerializeField] private UIButton _inventoryButton;
    [SerializeField] private UIButton _optionButton;
    [SerializeField] private UIButton _farmButton;

    [Header("장소 프리팹")]
    [SerializeField] private GameObject _villagePrefab;
    [SerializeField] private GameObject _monsterFarmPrefab;

    private GameObject _villageInstance;
    private GameObject _monsterFarmInstance;
    private Camera _gameCamera;
    private bool _isInMonsterFarm;

    [Header("Animation")]
    [SerializeField] private UISlideAnimation _slideAnimation;

    protected override void OnInit()
    {
        _inventoryButton.BindOnClickButtonEvent(OnClickInventory);
        _optionButton.BindOnClickButtonEvent(OnClickOption);
        _farmButton.BindOnClickButtonEvent(OnClickFarm);
    }

    protected override void OnOpened()
    {
        CreateLocations();

        if (_villageInstance != null)
        {
            ShowVillage();
        }
        else
        {
            ShowMonsterFarm();
        }

        if (_slideAnimation == null)
        {
            return;
        }

        _slideAnimation.SetHidden();
        _slideAnimation.SlideIn();
    }

    // 임시: 나중에 Model 소유처 정해지면 이동
    private InventoryModel _inventoryModelTest = new InventoryModel();

    protected override void OnClosed()
    {
        if (_villageInstance != null)
        {
            _villageInstance.SetActive(false);
        }

        if (_monsterFarmInstance != null)
        {
            _monsterFarmInstance.SetActive(false);
        }

        if (_gameCamera != null)
        {
            _gameCamera.gameObject.SetActive(true);
        }

        _isInMonsterFarm = false;
        _farmButton.ChangeText("팜");
    }

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

    private void OnClickFarm()
    {
        if (_isInMonsterFarm)
        {
            ShowVillage();
            return;
        }

        ShowMonsterFarm();
    }

    private void CreateLocations()
    {
        if (_gameCamera == null)
        {
            _gameCamera = Camera.main;
        }

        if (_villageInstance == null && _villagePrefab != null)
        {
            _villageInstance = Instantiate(_villagePrefab);
            _villageInstance.SetActive(false);
        }

        if (_monsterFarmInstance == null && _monsterFarmPrefab != null)
        {
            _monsterFarmInstance = Instantiate(_monsterFarmPrefab);
            _monsterFarmInstance.SetActive(false);
        }
    }

    private void ShowVillage()
    {
        if (_villageInstance == null)
        {
            return;
        }

        if (_gameCamera != null)
        {
            _gameCamera.gameObject.SetActive(false);
        }

        if (_monsterFarmInstance != null)
        {
            _monsterFarmInstance.SetActive(false);
        }

        _villageInstance.SetActive(true);

        _isInMonsterFarm = false;
        _farmButton.ChangeText("팜");
    }

    private void ShowMonsterFarm()
    {
        if (_monsterFarmInstance == null)
        {
            return;
        }

        if (_gameCamera != null)
        {
            _gameCamera.gameObject.SetActive(false);
        }

        if (_villageInstance != null)
        {
            _villageInstance.SetActive(false);
        }

        _monsterFarmInstance.SetActive(true);

        _isInMonsterFarm = true;
        _farmButton.ChangeText("마을 복귀");
    }

    private void OnClickOption()
    {
        GameManager.UI.OpenOptionUI();
    }
}
