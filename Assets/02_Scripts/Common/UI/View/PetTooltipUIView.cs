using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PetTooltipUIView : MonoBehaviour
{
    [SerializeField] private RectTransform _rootRect;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _elementText;
    [SerializeField] private TMP_Text _gradeText;
    [SerializeField] private TMP_Text _descriptionText;

    [SerializeField] private Vector2 _cursorOffset = new Vector2(20f, -20f);

    private RectTransform _canvasRect;
    private Canvas _canvas;
    private bool _isShowing = false;

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();

        if (_canvas != null)
        {
            _canvasRect = _canvas.GetComponent<RectTransform>();
        }

        HideTooltip();
    }

    private void Update()
    {
        if (_isShowing == false)
        {
            return;
        }

        FollowCursor();
    }

    public void ShowTooltip(PetSlotModel pet)
    {
        if (pet == null)
        {
            HideTooltip();
            return;
        }

        var petData = GameManager.DataTable.GetPetData(pet.PetDataId);
        if (petData == null)
        {
            HideTooltip();
            return;
        }

        _nameText.text = petData.Name;
        _elementText.text = petData.GetElementType().ToString();
        _gradeText.text = petData.GetGrade().ToString();
        _descriptionText.text = petData.Description;

        RefreshIcon(petData.IconPath);

        _isShowing = true;
        _canvasGroup.alpha = 1f;

        FollowCursor();
    }

    public void HideTooltip()
    {
        _isShowing = false;
        _canvasGroup.alpha = 0f;
    }

    private void RefreshIcon(string iconPath)
    {
        if (string.IsNullOrEmpty(iconPath) == true)
        {
            return;
        }

        Sprite iconSprite = Utils.ResourcesLoad<Sprite>(iconPath);
        if (iconSprite != null)
        {
            _iconImage.sprite = iconSprite;
        }
    }

    private void FollowCursor()
    {
        if (_canvasRect == null)
        {
            return;
        }

        if (Mouse.current == null)
        {
            return;
        }

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Camera uiCamera = null;

        if (_canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            uiCamera = _canvas.worldCamera;
        }

        Vector2 localPoint;
        bool isConverted = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRect, mousePosition, uiCamera, out localPoint);

        if (isConverted == false)
        {
            return;
        }

        _rootRect.anchoredPosition = localPoint + _cursorOffset;
    }
}