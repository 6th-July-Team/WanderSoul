using UnityEngine;

public static class VFXSpawner
{
    public static ParticleComponent SpawnVFX(string address, Vector3 spawnPos, Quaternion rotation, float scale = 1f)
    {
        ParticleComponent effect = CreateParticle(address);

        if (effect == null)
        {
            return null;
        }

        effect.transform.position = spawnPos;
        effect.transform.rotation = rotation;
        effect.SetScale(Vector3.one * scale);

        effect.Play();
        return effect;
    }

    public static ParticleComponent SpawnAttachedVFX(string address
        , Transform parent, Vector3 localPos, Quaternion localRot
        , float scale = 1f)

    {
        ParticleComponent effect = CreateParticle(address);

        if (effect == null)
        {
            return null;
        }

        effect.transform.SetParent(parent, false);
        effect.transform.position = localPos;
        effect.transform.rotation = localRot;
        effect.SetScale(Vector3.one * scale);

        effect.Play();
        return effect;
    }

    private static ParticleComponent CreateParticle(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            return null;
        }

        var instance = GameManager.Pool.SpawnFromPool<ParticleComponent>(address);

        return instance;
    }
}
