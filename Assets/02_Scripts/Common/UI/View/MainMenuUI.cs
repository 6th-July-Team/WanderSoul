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
        ShowVillage();

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
        CloseLocationPanels();

        if (_inventoryModelTest.ItemList.Count == 0)
        {
            CreateTestItemList();
        }

        GameManager.UI.OpenInventoryUI(_inventoryModelTest);
    }

    private void CloseLocationPanels()
    {
        if (_villageInstance != null)
        {
            var villageInput = _villageInstance.GetComponentInChildren<VillageClickInput>(true);

            if (villageInput != null)
            {
                villageInput.CloseAllPanels();
            }
        }

        if (_monsterFarmInstance == null)
        {
            return;
        }

        var farmInput = _monsterFarmInstance.GetComponentInChildren<FarmClickInput>(true);

        if (farmInput != null)
        {
            farmInput.CloseAllPanels();
        }
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
        CloseLocationPanels();

        if (_isInMonsterFarm)
        {
            ShowVillage();
            return;
        }

        ShowMonsterFarm();
    }

    private void CreateLocations()
    {
        if (_villageInstance != null && _monsterFarmInstance != null)
        {
            return;
        }

        if (_villagePrefab == null || _monsterFarmPrefab == null)
        {
            Debug.LogWarning("마을 또는 몬스터 팜 프리팹이 연결되지 않았습니다.");
            return;
        }

        _gameCamera = Camera.main;

        _villageInstance = Instantiate(_villagePrefab);
        _monsterFarmInstance = Instantiate(_monsterFarmPrefab);

        _villageInstance.SetActive(false);
        _monsterFarmInstance.SetActive(false);
    }

    private void ShowVillage()
    {
        if (_villageInstance == null || _monsterFarmInstance == null)
        {
            return;
        }

        if (_gameCamera != null)
        {
            _gameCamera.gameObject.SetActive(false);
        }

        _monsterFarmInstance.SetActive(false);
        _villageInstance.SetActive(true);

        _isInMonsterFarm = false;
        _farmButton.ChangeText("팜");
    }

    private void ShowMonsterFarm()
    {
        if (_villageInstance == null || _monsterFarmInstance == null)
        {
            return;
        }

        if (_gameCamera != null)
        {
            _gameCamera.gameObject.SetActive(false);
        }

        _villageInstance.SetActive(false);
        _monsterFarmInstance.SetActive(true);

        _isInMonsterFarm = true;
        _farmButton.ChangeText("마을 복귀");
    }

    private void OnClickOption()
    {
        CloseLocationPanels();

        GameManager.UI.OpenOptionUI();
    }
}
