using UnityEngine;

public class PetAreaBuffInstance : MonoBehaviour
{
    private StatusEffectData _effectData;

    private IStatModifierReceiver _playerAdapter;

    private ModifierHandle _handle;

    public void Init(StatusEffectData effectData, IStatModifierReceiver playerAdapter)
    {
        _effectData = effectData;
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
