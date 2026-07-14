
public class PetSkillMaker
{
    public PetCombatController CreateCombatController(string id, PetStatController statController)
    {
        PetCombatController build = null;

        // TODO(김익환): SOSkillDefinition는 데이터 드리븐으로 각 스킬에 맞는 데이터 넘기기.
        if (id == "테스트 펫 아이디")
        {
            build = new(
                new PetActiveSkill(GameManager.Instance.TestSOPetSkillInfo, new PetProjectileSkill(), statController)
                , null
                , null);
        }
        build = new(
                new PetActiveSkill(GameManager.Instance.TestSOPetSkillInfo, new PetProjectileSkill(), statController)
                , null
                , null);
        return build;
    }
}
