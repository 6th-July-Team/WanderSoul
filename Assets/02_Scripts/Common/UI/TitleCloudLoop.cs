using UnityEngine;

public class TitleCloudLoop : MonoBehaviour
{
    [Header("Cloud References")]
    [SerializeField] private RectTransform _largeCloud1;
    [SerializeField] private RectTransform _cloud1;
    [SerializeField] private RectTransform _largeCloud2;
    [SerializeField] private RectTransform _cloud2;

    [Header("Horizontal Loop")]
    [SerializeField] private float _horizontalSpeed = 18f;
    [SerializeField] private float _firstLargeCloudStartX = -757f;
    [SerializeField] private float _firstCloudStartX = -457f;
    [SerializeField] private float _secondLargeCloudStartX = -2863f;
    [SerializeField] private float _secondCloudStartX = -2563f;
    [SerializeField] private float _followStartX = 792f;
    [SerializeField] private bool _playOnEnable = true;

    [Header("Vertical Drift")]
    [SerializeField] private float _largeCloudVerticalRange = 8f;
    [SerializeField] private float _cloudVerticalRange = 12f;
    [SerializeField] private float _verticalCycleDuration = 20f;
    [SerializeField] private bool _useUnscaledTime = true;

    private float _largeCloud1BaseY;
    private float _cloud1BaseY;
    private float _largeCloud2BaseY;
    private float _cloud2BaseY;

    private float _recycleX;
    private float _elapsedTime;
    private bool _isSecondGroupMoving;
    private bool _isInitialized;
    private bool _isPlaying;

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        if (!_isInitialized)
        {
            Initialize();
        }

        ResetAnimation();

        if (_playOnEnable)
        {
            PlayAnimation();
        }
    }

    private void Update()
    {
        if (!_isInitialized || !_isPlaying)
        {
            return;
        }

        float deltaTime = _useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        float moveDistance = _horizontalSpeed * deltaTime;

        MoveGroup(_largeCloud1, _cloud1, moveDistance);

        if (!_isSecondGroupMoving && _largeCloud1.anchoredPosition.x >= _followStartX)
        {
            _isSecondGroupMoving = true;
        }

        if (_isSecondGroupMoving)
        {
            MoveGroup(_largeCloud2, _cloud2, moveDistance);
        }

        RecycleGroupIfNeeded(_largeCloud1, _cloud1);

        if (_isSecondGroupMoving)
        {
            RecycleGroupIfNeeded(_largeCloud2, _cloud2);
        }

        _elapsedTime += deltaTime;
        ApplyVerticalDrift();
    }

    private void Initialize()
    {
        if (_largeCloud1 == null || _cloud1 == null || _largeCloud2 == null || _cloud2 == null)
        {
            Debug.LogWarning("TitleCloudLoop의 구름 RectTransform 연결을 확인해 주세요.", this);
            enabled = false;
            return;
        }

        _largeCloud1BaseY = _largeCloud1.anchoredPosition.y;
        _cloud1BaseY = _cloud1.anchoredPosition.y;
        _largeCloud2BaseY = _largeCloud2.anchoredPosition.y;
        _cloud2BaseY = _cloud2.anchoredPosition.y;

        float groupSpacing = _followStartX - _secondLargeCloudStartX;
        _recycleX = _followStartX + groupSpacing;
        _isInitialized = true;
    }

    private void ResetAnimation()
    {
        SetPosition(_largeCloud1, _firstLargeCloudStartX, _largeCloud1BaseY);
        SetPosition(_cloud1, _firstCloudStartX, _cloud1BaseY);
        SetPosition(_largeCloud2, _secondLargeCloudStartX, _largeCloud2BaseY);
        SetPosition(_cloud2, _secondCloudStartX, _cloud2BaseY);

        _elapsedTime = 0f;
        _isSecondGroupMoving = _firstLargeCloudStartX >= _followStartX;
    }

    public void PlayAnimation()
    {
        if (!_isInitialized)
        {
            Initialize();
        }

        _isPlaying = _isInitialized;
    }

    public void StopAnimation(bool resetPosition)
    {
        _isPlaying = false;

        if (_isInitialized && resetPosition)
        {
            ResetAnimation();
            ApplyVerticalDrift();
        }
    }

    private void MoveGroup(RectTransform largeCloud, RectTransform cloud, float distance)
    {
        Vector2 largePosition = largeCloud.anchoredPosition;
        largePosition.x += distance;
        largeCloud.anchoredPosition = largePosition;

        Vector2 cloudPosition = cloud.anchoredPosition;
        cloudPosition.x += distance;
        cloud.anchoredPosition = cloudPosition;
    }

    private void RecycleGroupIfNeeded(RectTransform largeCloud, RectTransform cloud)
    {
        if (largeCloud.anchoredPosition.x < _recycleX)
        {
            return;
        }

        float overflow = largeCloud.anchoredPosition.x - _recycleX;
        SetPosition(largeCloud, _secondLargeCloudStartX + overflow, largeCloud.anchoredPosition.y);
        SetPosition(cloud, _secondCloudStartX + overflow, cloud.anchoredPosition.y);
    }

    private void ApplyVerticalDrift()
    {
        float safeDuration = Mathf.Max(0.1f, _verticalCycleDuration);
        float phase = _elapsedTime * Mathf.PI * 2f / safeDuration;
        float firstGroupWave = Mathf.Sin(phase);
        float secondGroupWave = Mathf.Sin(phase + Mathf.PI);

        SetY(_largeCloud1, _largeCloud1BaseY + firstGroupWave * _largeCloudVerticalRange);
        SetY(_cloud1, _cloud1BaseY - firstGroupWave * _cloudVerticalRange);
        SetY(_largeCloud2, _largeCloud2BaseY + secondGroupWave * _largeCloudVerticalRange);
        SetY(_cloud2, _cloud2BaseY - secondGroupWave * _cloudVerticalRange);
    }

    private void SetPosition(RectTransform target, float x, float y)
    {
        target.anchoredPosition = new Vector2(x, y);
    }

    private void SetY(RectTransform target, float y)
    {
        Vector2 position = target.anchoredPosition;
        position.y = y;
        target.anchoredPosition = position;
    }
}
