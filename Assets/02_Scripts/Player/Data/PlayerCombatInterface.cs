
public interface IPlayerSkillExecution
{
    void Execute(PlayerSkillUseContext context, PlayerSkillData SkillData, float damage, float coolTimeReductionOfStat);
}