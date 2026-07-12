
public interface ISkillExecution
{
    bool CanExecute(SkillUseContext context);
    void Execute(SkillUseContext context, float damage);
}