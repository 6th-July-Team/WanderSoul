using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ScholarOrb : MonoBehaviour
{
    [Header("Follow")]
    [SerializeField] private float _followingTime = 0.1f;
    private Transform _followPoint;

    [Header("Up Down")]
    [SerializeField] private float _height = 0.3f;
    [SerializeField] private float _speed = 2f;

    private Vector3 _followVelocity;
    private Vector3 _destination;


    public void Init(Transform followPoint)
    {
        _followPoint = followPoint;

        transform.position = _destination = followPoint.position;
        GetComponent<PariclePaletteController>().ApplyPalette(
            Utils.GetEffectColorByPetElement(GameManager.PetParty.GetPriorityPetElement()));
        
    }


    void LateUpdate()
    {
        _destination = Vector3.SmoothDamp(_destination, _followPoint.position, ref _followVelocity, _followingTime);

        float yOffset = Mathf.Sin(Time.time * _speed) * _height;

        transform.position = _destination + Vector3.up * yOffset;
    }
}