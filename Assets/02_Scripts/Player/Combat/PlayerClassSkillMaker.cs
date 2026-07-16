using UnityEngine;
using System;
using System.Collections.Generic;
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
                new PlayerSkill("test", new MercenarySlash(), _manaPool, statController)
                , new PlayerSkill("test", new MercenaryWhip(), _manaPool, statController)
                , new PlayerSkill("test", new MercenaryDanceStorm(), _manaPool, statController));
        }
        build = new(
                new PlayerSkill("test", new MercenarySlash(), _manaPool, statController)
                , new PlayerSkill("test", new MercenaryWhip(), _manaPool, statController)
                , new PlayerSkill("test", new MercenaryDanceStorm(), _manaPool, statController));
        return build;
    }
}
