using UnityEngine;

public class WagonBoundary : MonoBehaviour
{
    private WagonViewModel _viewModel;

    private float _warningDuration = 10f;
    private float _warningTimer = 0f;

    private bool _isInitalized = false;


    public void Init(WagonViewModel viewMdodel)
    {
        _viewModel = viewMdodel;
        _isInitalized = true;
    }

    private void Update()
    {
        if (!_isInitalized)
            return;

        if(_viewModel.GetIsWarningActive)
        {
            _warningTimer += GameManager.Time.GameDeltaTime;
            _viewModel.SetWarningTime(_warningTimer);

            if ( _warningTimer >= _warningDuration)
            {
                _warningTimer = 0f;
                GameManager.Convoy.FaildConvoy(ConvoyFailReason.OutOfWagonArea);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _viewModel.SetWarningActive(false);
            _warningTimer = 0f;
            _viewModel.SetWarningTime(0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _viewModel.SetWarningActive(true);
        }
    }
}
