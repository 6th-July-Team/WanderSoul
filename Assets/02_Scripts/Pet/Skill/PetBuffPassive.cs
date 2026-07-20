using UnityEngine;

public class PetBuffPassive : IPetPassiveSkillExecution
{
    private StatusEffectData _effectData;
    private PetPassiveSkillData _passiveSkillData;


    private IStatModifierReceiver _playerAdapter;
    private IStatModifierReceiver _petAdapter;
    private ModifierHandle _handle;

    public PetBuffPassive(StatusEffectData effectData, PetPassiveSkillData passiveSkillData
        , IStatModifierReceiver playerAdapter, IStatModifierReceiver petAdapter)
    {
        _effectData = effectData;
        _passiveSkillData = passiveSkillData;

        _playerAdapter = playerAdapter;
        _petAdapter = petAdapter;
    }

    public void Activate()
    {
        switch (_passiveSkillData.GetTargetType())
        {
            case TargetType.PlayerAndPet:
                {
                    StatModifier playerAndPetModifier
                        = new StatModifier(_effectData.GetStat(), _effectData.GetOperation(), _effectData.Value);

                    _handle = _playerAdapter.AddModifier(playerAndPetModifier);
                    _handle = _petAdapter.AddModifier(playerAndPetModifier);
                }
                break;

            case TargetType.Player:
                {
                    StatModifier playerModifier = new StatModifier(_effectData.GetStat(), _effectData.GetOperation(), _effectData.Value);
                    _handle = _playerAdapter.AddModifier(playerModifier);
                }
                break;

            case TargetType.Pet:
                {
                    StatModifier petModifier = new StatModifier(_effectData.GetStat(), _effectData.GetOperation(), _effectData.Value);
                    _handle = _petAdapter.AddModifier(petModifier);
                }
                break;

            default:
                Debug.LogWarning("Unsupported target type for PetBuffPassive: " + _passiveSkillData.GetTargetType());
                break;
        }
    }

    public void Deactivate()
    {
        _playerAdapter.RemoveModifier(_handle);
    }
}
