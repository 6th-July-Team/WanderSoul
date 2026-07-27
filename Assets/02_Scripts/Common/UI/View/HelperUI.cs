using UnityEngine;
using UnityEngine.UI;

public class HelperUI : BaseUI
{
    [SerializeField] private Image Image_Background;

    [SerializeField] private GameObject[] Page;
    [SerializeField] private Button Button_Prev;
    [SerializeField] private Button Button_Next;


    private int _currentPage;

    protected override void OnInit()
    {
        if (Button_Prev != null)
        {
            Button_Prev.onClick.AddListener(ShowPrev);
        }

        if (Button_Next != null)
        {
            Button_Next.onClick.AddListener(ShowNext);
        }
    }

    protected override void OnOpened()
    {
        _currentPage = 0;
        ShowPage(_currentPage);
    }

    private void ShowPrev()
    {
        _currentPage--;
        ShowPage(_currentPage);
    }

    private void ShowNext()
    {
        _currentPage++;
        ShowPage(_currentPage);
    }

    private void ShowPage(int index)
    {
        if (Page == null || Page.Length == 0)
        {
            ExitUI();
            return;
        }

        if (index < 0 || index >= Page.Length)
        {
            ExitUI();
            return;
        }

        for (int i = 0; i < Page.Length; i++)
        {
            if (Page[i] == null)
            {
                continue;
            }

            Page[i].SetActive(i == index);
        }
    }

    private void ExitUI()
    {
        GameManager.UI.CloseUI(UIType.HelperUI);
    }
}
