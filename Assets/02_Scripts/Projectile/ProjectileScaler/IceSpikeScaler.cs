using UnityEngine;

public class IceSpikeScaler : MonoBehaviour, IProjectileScaler
{
    [SerializeField] private GameObject Beamin;
    [SerializeField] private GameObject Beam;
    [SerializeField] private GameObject Impact;
    [SerializeField] private GameObject Circle;
    [SerializeField] private GameObject DarkParticles;
    [SerializeField] private GameObject Particles;
    [SerializeField] private GameObject BeamFlare;
    [SerializeField] private GameObject Stones;
    [SerializeField] private GameObject Flakes;

    public void SetScale(Vector3 scale)
    {
        Beamin.transform.localScale = scale;
        Beam.transform.localScale = scale;
        Impact.transform.localScale = scale;
        Circle.transform.localScale = scale;
        Particles.transform.localScale = scale;
        DarkParticles.transform.localScale = scale;
        Particles.transform.localScale = scale;
        BeamFlare.transform.localScale = scale;
        Stones.transform.localScale = scale;
        Flakes.transform.localScale = scale;
    }

}
