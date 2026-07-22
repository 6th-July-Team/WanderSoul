using UnityEngine;

public class ScholarBarrier : MonoBehaviour, IBarrierable, IDamageable
{
    private float _absorbAmount;
    private float _duration;

    public bool IsAlive => _absorbAmount > 0 && _duration > 0;

    public EntityType EntityType => EntityType.Player;

    public Vector3 Position => transform.position;

    public Transform Transform => this.transform;

    public void Init(PlayerSkillData skillData)
    {
        _absorbAmount = skillData.BarrierAbsorbAmount;
        _duration = 5f;// skillData.Duration;
        transform.localScale = Vector3.one * 7f;//skillData.Radius;
    }

    private void Update()
    {
        if(GameManager.Time.IsPaused)
            return;

        _duration -= Time.deltaTime;

        if(_duration <= 0)
        {
            BarrierDestory();
        }
    }

    public void AbsorbDamage(float damage)
    {
        _absorbAmount -= damage;

        if (_absorbAmount <= 0)
        {
            BarrierDestory();
        }
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        AbsorbDamage(damageInfo.DamageAmount);
    }

    private void BarrierDestory()
    {
        // TOODO(김익환): 결계 파괴 이펙트
        // 주변 적 탐색 후 데미지 입히기 결계내부까지 ㅇㅇ
        Destroy(gameObject);
    }
}
