using UnityEngine;

public class WagonMonsterCounter : MonoBehaviour
{
    private WagonViewModel _wagonViewModel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _wagonViewModel.IncreaseEnemyCount();

            // TODO: Enemy의 OnEnemyDieEvent 이벤트 추가시 주석 해제
            // other.GetComponent<Enemy>().OnEnemyDieEvent =+ () => _wagonViewModel.IncreaseEnemyCount();
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
