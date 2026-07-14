
public interface IPlayerSkillExecution
{
    bool CanExecute(PlayerSkillUseContext context);
    void Execute(PlayerSkillUseContext context, float damage);
}