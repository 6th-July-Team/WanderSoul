using UnityEngine;

public class PetAreaBuffPassive : IPetPassiveSkillExecution
{
    private StatusEffectData _effectData;
    private PetPassiveSkillData _passiveSkillData;

    private IStatModifierReceiver _playerAdapter;
    private IPet _petEntity;

    private PetAreaBuffInstance _buffInstance;

    public PetAreaBuffPassive(StatusEffectData effectData, PetPassiveSkillData passiveSkillData
        , IStatModifierReceiver playerAdapter, IPet petEntity)
    {
        _effectData = effectData;
        _passiveSkillData = passiveSkillData;

        _playerAdapter = playerAdapter;
        _petEntity = petEntity;
    }

    public void Activate()
    {
        _buffInstance = Object.Instantiate(Utils.ResourcesLoad<PetAreaBuffInstance>("Pet/AreaBuff"), _petEntity.Transform);
        _buffInstance.Init(_effectData, _playerAdapter, _passiveSkillData);
    }

    public void Deactivate()
    {
        _buffInstance.Release();
        Object.Destroy(_buffInstance);
    }
}
