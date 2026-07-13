using UnityEngine;

public class WagonBoundary : MonoBehaviour
{
    private WagonViewModel _viewModel;

    private float _warningDuration = 20f;
    private float _warningTimer = 0f;
    private bool _isWarningActive = false;

    private void Awake()
    {
        // TODO(김익환): 데이터 드리븐 추가 시 주석 해제
        //_warningDuration = GameManager.DataTable.GetData();
    }

    private void Update()
    {
        if( _isWarningActive)
        {
            _warningTimer += GameManager.Time.GameDeltaTime;
            _viewModel.SetWarningTime(_warningTimer);

            if ( _warningTimer >= _warningDuration)
            {
                // TODO(김익환): 경고 시간 초과 시 처리 로직 추가

                _warningTimer = 0f;
                _isWarningActive = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isWarningActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isWarningActive = false;
            _warningTimer = 0f;
            _viewModel.SetWarningTime(0f);
        }
    }
}
