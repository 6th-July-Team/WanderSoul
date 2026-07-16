using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class SkillRangeIndicator : MonoBehaviour
{
    [SerializeField] private DecalProjector _decalProjector;
    [SerializeField] private float _projectionDepth = 1f;
    [SerializeField] private float _groundOffset = 0.5f;

    private void Awake()
    {
        _decalProjector.enabled = false;
    }

    public void Show(Vector3 center, float radius)
    {
        transform.position = center;   // + Vector3.up * _groundOffset;

        float diameter = radius * 2f;

        _decalProjector.size = new Vector3(diameter, diameter, _projectionDepth);

        _decalProjector.enabled = true;
    }

    public void Show(Vector3 center, Vector3 direct, float height, float width)
    {
        Quaternion rotation = Quaternion.LookRotation(direct, Vector3.up);

        Vector3 pos = center + direct * (height * 0.5f);

        transform.position = pos;
        transform.rotation = rotation;

        _decalProjector.size = new Vector3(width, height, _projectionDepth);
        _decalProjector.enabled = true;
    }

    public void Hide()
    {
        _decalProjector.enabled = false;
    }
}