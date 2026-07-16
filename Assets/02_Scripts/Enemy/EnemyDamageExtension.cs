using System.Collections.Generic;
using UnityEngine;

public static class EnemyDamageExtension
{
    private static void ApplyDamage(this EnemyViewModel viewModel, IDamageable damageableTarget, DamageInfo damageInfo)
    {
        if (damageableTarget == null || damageableTarget.IsAlive == false)
        {
            return;
        }

        damageableTarget.TakeDamage(damageInfo);
    }

    public static void MeleeTypeAttack(this EnemyViewModel viewModel, IReadOnlyList<IDamageable> damageableTargets, Vector3 attackerPosition)
    {
        for (int i = damageableTargets.Count - 1; i >= 0; i--)
        {
            IDamageable damageableTarget = damageableTargets[i];

            if(damageableTarget == null)
            {
                continue;
            }

            Vector3 hitDirection = (damageableTarget.Position - attackerPosition).normalized;

            DamageInfo damageInfo = new(viewModel.Attack, hitDirection);

            viewModel.ApplyDamage(damageableTarget, damageInfo);
        }
    }

    public static void ProjectileTypeAttack(this EnemyViewModel viewModel, IDamageable damageableTarget, Vector3 hitDirection)
    {
        DamageInfo damageInfo = new(viewModel.Attack, hitDirection);

        viewModel.ApplyDamage(damageableTarget, damageInfo);
    }

    public static void StealTypeAttack(this EnemyViewModel viewModel)
    {

    }
}