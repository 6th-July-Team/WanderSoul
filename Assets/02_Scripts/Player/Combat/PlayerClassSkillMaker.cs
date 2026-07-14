using UnityEngine;

public class PlayerClassSkillMaker
{
    private ManaPool _manaPool;
    public PlayerClassSkillMaker(ManaPool manaPool)
    {
        _manaPool = manaPool;
    }

    public PlayerClassSkillBuild CreateSkillBuild(string id, PlayerStatController statController)
    {
        PlayerClassSkillBuild build = null;

        // TODO(김익환): SOSkillDefinition는 데이터 드리븐으로 각 스킬에 맞는 데이터 넘기기.
        if (id == "테스트 직업 아이디")
        {
            build = new(
                new PlayerSkill("test", new ScholarBasicExecution(GameManager.PetParty), _manaPool, statController)
                , null
                , null);
        }
        build = new(
                new PlayerSkill("test", new ScholarBasicExecution(GameManager.PetParty), _manaPool, statController)
                , null
                , null);
        return build;
    }
}
