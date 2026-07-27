using UnityEngine;
using UnityEngine.UI;

public class HelperUI : MonoBehaviour
{
    [SerializeField] private Image Image_Background;

    [SerializeField] private GameObject[] Page;
    [SerializeField] private Button Button_Prev;
    [SerializeField] private Button Button_Next;


    private int _currentPage;



    private void Awake()
    {
        Button_Prev.onClick.AddListener(ShowPrev);
        Button_Next.onClick.AddListener(ShowNext);
    }

    private void OnEnable()
    {
        _currentPage = 0;
        ShowPage(_currentPage);
    }

    private void ShowPrev()
    {
        if (_currentPage > 0)
        {
            _currentPage--;
        }

        ShowPage(_currentPage);
    }

    private void ShowNext()
    {
        if(_currentPage < 3)
        {
            _currentPage++;
        }

        ShowPage(_currentPage);
    }

    private void ShowPage(int index)
    {
        for(int i = 0; i < Page.Length; i++)
        {
            if (i == index)
            {
                Page[i].SetActive(true);
            }
            else
            {
                Page[i].SetActive(false);
            }
        }
    }

    private void ExitUI()
    {
        gameObject.SetActive(false);
        // TODO : UIManager 종료
    }
}
