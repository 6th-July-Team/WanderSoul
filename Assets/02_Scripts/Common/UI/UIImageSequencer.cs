using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIImageSequencer : MonoBehaviour
{
    [Header("Sequence")]
    [SerializeField] private Sprite[] _sprites;  
    [SerializeField] private float _sequenceInterval = 0.1f;
    [SerializeField] private bool _isLoop = true;

    private Image _image;
    private CancellationTokenSource _cancelToken;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        PlayAnimation().Forget();
    }

    private void OnDisable()
    {
        StopAnimation();
    }

    private void OnDestroy()
    {
        StopAnimation();
    }

    public async UniTaskVoid PlayAnimation()
    {
        if (_sprites == null || _sprites.Length == 0)
        {
            return;
        }

        StopAnimation();
        _cancelToken = new CancellationTokenSource();
        var token = _cancelToken.Token;

        int currentIndex = 0;

        while (true)
        {
            _image.sprite = _sprites[currentIndex];

            bool isCanceled = await UniTask.Delay(
                TimeSpan.FromSeconds(_sequenceInterval),
                cancellationToken: token).SuppressCancellationThrow();

            if (isCanceled == true)
            {
                return;
            }

            currentIndex++;

            if (currentIndex >= _sprites.Length)
            {
                if (_isLoop == true)
                {
                    currentIndex = 0;
                }
                else
                {
                    break;
                }
            }
        }
    }

    public void StopAnimation()
    {
        if (_cancelToken != null)
        {
            _cancelToken.Cancel();
            _cancelToken.Dispose();
            _cancelToken = null;
        }
    }
}