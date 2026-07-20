

public interface IPetActiveSkillExecution
{
    void Execute(PetSkillUseContext context, System.Action OnEndSkill);
}

public interface IPetPassiveSkillExecution
{
    void Activate();
    void Deactivate();
}

public interface IUpdatablePetPassive
{
    void Update(float deltaTime);
}