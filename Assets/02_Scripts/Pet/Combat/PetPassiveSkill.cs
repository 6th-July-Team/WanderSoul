

public class PetPassiveSkill
{
    private PetPassiveSkillData _skillData;
    private IPetPassiveSkillExecution _execution;

    private bool _isActive;

    public PetPassiveSkill(PetPassiveSkillData skillData, IPetPassiveSkillExecution execution)
    {
        _skillData = skillData;
        _execution = execution;
    }

    public void Activate()
    {
        if (_isActive)
            return;

        _isActive = true;
        _execution.Activate();
    }

    public void Update(float deltaTime)
    {
        if (!_isActive)
            return;

        if( _execution is IUpdatablePetPassive updatable)
        {
            updatable.Update(deltaTime);
        }
    }

    public void Deactivate()
    {
        if (!_isActive)
            return;

        _isActive = false;
        _execution.Deactivate();
    }
}
