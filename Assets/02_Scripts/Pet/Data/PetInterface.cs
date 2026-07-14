

public interface IPetActiveSkillExecution
{
    void Execute(PetSkillUseContext context, float damage, System.Action OnEndSkill);
}