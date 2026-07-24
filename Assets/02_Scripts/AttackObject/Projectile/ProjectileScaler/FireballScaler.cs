using UnityEngine;

public class FireballScaler : MonoBehaviour, IProjectileScaler
{
    [SerializeField] private Transform Impact;
    [SerializeField] private Transform Cross;
    [SerializeField] private Transform Beam;
    [SerializeField] private Transform Rings;
    [SerializeField] private Transform Particles;
    [SerializeField] private Transform Smoke;

    public void SetScale(Vector3 scale)
    {
        Impact.localScale = scale;
        Cross.localScale = scale;
        Beam.localScale = scale;
        Rings.localScale = scale;
        Particles.localScale = scale;
        Smoke.localScale = scale;
    }
}
