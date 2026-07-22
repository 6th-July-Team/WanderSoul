using UnityEngine;

public class PlayerDropObjectCollider : MonoBehaviour , IPositionProvider
{
    public Vector3 Position => this.transform.position;
    public Transform Transform => this.transform;

    private PlayerViewModel _viewModel;
    private SphereCollider _sphereCollider;

    private void Awake()
    {
        _viewModel = GameManager.Network.RequestCreatePlayer();
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.radius = _viewModel.GetMagnetRadius;
    }

    private void OnEnable()
    {
        _viewModel.OnPropertyChanged_View += OnPropertyChanged;
    }

    private void OnDisable()
    {
        _viewModel.OnPropertyChanged_View -= OnPropertyChanged;
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        throw new System.NotImplementedException();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyDropObject>(out var enemyDropObject))
        {
            enemyDropObject.StartFollowPlayer(this);
        }
    }

    private void OnPropertyChanged(string propertyName)
    {
        if(propertyName == nameof(PlayerModel.MagnetRadius))
        {
            _sphereCollider.radius = _viewModel.GetMagnetRadius;
        }
    }
}
