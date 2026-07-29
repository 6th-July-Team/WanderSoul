using System;
using UnityEngine;

public class ScholarBarrier : MonoBehaviour//, IBarrierable
{
    private float _totalDamage;
    private float _durability;
    private float _duration;
    private float _radius;


    private Collider[] _colliders = new Collider[64];

    private EffectColorType _effectColor;

    public void Init(PlayerSkillData skillData, PetElement element)
    {
        _durability = skillData.BarrierAbsorbAmount;
        _duration = skillData.Duration;
        _radius = skillData.Radius;

        transform.localScale = Vector3.one * _radius * 2;

        _effectColor = Utils.GetEffectColorByPetElement(element);
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

    public float AbsorbDamage(float damage)
    {
        float absorbedDamage = Mathf.Min(damage, _durability);

        _durability -= absorbedDamage;
        _totalDamage += absorbedDamage;

        if (_durability <= 0)
        {
            BarrierDestory();
        }

        return damage - absorbedDamage;
    }

    private void BarrierDestory()
    {
        var effect = VFXSpawner.SpawnVFX("Scholar/BarrierExplosionEffect"
            , transform.position, Quaternion.identity, _radius * 2);


        effect.GetComponent<PariclePaletteController>().ApplyPalette(_effectColor);

        int count = SearchUtil.FindTargetSphere(transform.position, transform.localScale.x / 2f, LayerMask.GetMask("Enemy"), _colliders);
        for (int i = 0; i < count; i++)
        {
            if(_colliders[i].TryGetComponent(out IDamageable damageable))
            {

                DamageInfo damageInfo = new DamageInfo(_totalDamage, damageable.Position - transform.position, DamageType.None);
                damageable.TakeDamage(damageInfo);
            }
        }

        Destroy(gameObject);
    }
}
