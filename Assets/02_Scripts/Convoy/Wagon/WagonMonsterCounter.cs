using UnityEngine;

public class WagonMonsterCounter : MonoBehaviour
{
    private WagonViewModel _wagonViewModel;
    
    public void Init(WagonViewModel wagonViewModel)
    {
        _wagonViewModel = wagonViewModel;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _wagonViewModel.IncreaseEnemyCount();

            other.GetComponent<EnemyView>().OnEnemyDied += () => _wagonViewModel.IncreaseEnemyCount();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _wagonViewModel.ReduceEnemyCount();
        }
    }
}
