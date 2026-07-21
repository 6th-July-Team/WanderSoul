using UnityEngine;

public class PetAreaBuffPassive : IPetPassiveSkillExecution
{
    private StatusEffectData _effectData;
    private PetPassiveSkillData _passiveSkillData;

    private IStatModifierReceiver _playerAdapter;
    private IPet _petEntity;

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
        var a = Object.Instantiate(Utils.ResourcesLoad<PetAreaBuffInstance>("AreaBuff"), _petEntity.Transform);
        a.Init(_effectData, _playerAdapter);
    }

    public void Deactivate()
    {
        throw new System.NotImplementedException();
    }
}
