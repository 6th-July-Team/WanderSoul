using UnityEngine;

public class PetAreaBuffInstance : MonoBehaviour
{
    private StatusEffectData _effectData;

    private IStatModifierReceiver _playerAdapter;

    private ModifierHandle _handle;

    private SphereCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.isTrigger = true;
    }

    public void Init(StatusEffectData effectData, IStatModifierReceiver playerAdapter, PetPassiveSkillData petPassiveSkillData)
    {
        _effectData = effectData;
        _collider.radius = petPassiveSkillData.Radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IPlayer>(out var player))
        {
            StatModifier playerModifier
                        = new StatModifier(_effectData.GetStat(), _effectData.GetOperation(), _effectData.Value);

            _handle = _playerAdapter.AddModifier(playerModifier);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<IPlayer>(out var player))
        {
            _playerAdapter.RemoveModifier(_handle);
        }
    }
}
