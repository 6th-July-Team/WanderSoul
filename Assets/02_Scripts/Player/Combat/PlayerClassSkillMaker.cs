

public class PlayerClassSkillMaker
{
    private PlayerViewModel _playerViewModel;
    private PlayerSkillModifier _playerSkillModifier;

    public PlayerClassSkillMaker(PlayerViewModel playerViewModel, PlayerSkillModifier playerSkillModifier)
    {
        _playerViewModel = playerViewModel;
        _playerSkillModifier = playerSkillModifier;
    }

    public PlayerClassSkillBuild CreateSkillBuild(string id, PlayerStatController statController)
    {
        PlayerClassSkillBuild build = null;

        // TODO(김익환): SOSkillDefinition는 데이터 드리븐으로 각 스킬에 맞는 데이터 넘기기.
        if (id == "테스트 직업 아이디")
        {
            

        }

        build = new(
               new PlayerSkill("test", new MercenarySlash(), _playerViewModel, statController, _playerSkillModifier)
               , new PlayerSkill("test", new MercenaryWhip(), _playerViewModel, statController, _playerSkillModifier)
               , new PlayerSkill("test", new MercenaryDanceStorm(), _playerViewModel, statController, _playerSkillModifier)
               );


        return build;
    }
}
