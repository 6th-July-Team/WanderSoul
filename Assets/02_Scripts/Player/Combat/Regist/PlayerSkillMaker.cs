

using UnityEngine;

public class PlayerSkillMaker
{
    private PlayerClassSkillBuild _build;
    private PlayerStatController _statController;

    private PlayerSkillRegistry _skillRegistry;
    private PlayerViewModel _playerViewModel;


    private IPetPartyReader _petPartyReader;
    private IStatusEffectReceiver[] _petStatusEffectReceviers;

    private StatusEffectMaker _statusEffectMaker;


    public PlayerSkillMaker(PlayerSkillRegistry skillRegistry, IPetPartyReader petPartyReader, PlayerViewModel playerViewModel
        , StatusEffectMaker statusEffectMaker)
    {
        _skillRegistry = skillRegistry;
        _petPartyReader = petPartyReader;

        _playerViewModel = playerViewModel;
        _statusEffectMaker = statusEffectMaker;
    }

    public PlayerClassSkillBuild CreateSkillBuild(string playerId, PlayerStatController statController
        , IStatusEffectReceiver[] petStatusEffectReceviers)
    {
        _statController = statController;
        _petStatusEffectReceviers = petStatusEffectReceviers;

        _build = new();

        PlayerClassData playerClassData = GameManager.DataTable.GetPlayerClassData(playerId);

        PlayerSkillData basicSkillData = GameManager.DataTable.GetPlayerSkillData(playerClassData.BasicSkillId);
        PlayerSkillData specialSkillData = GameManager.DataTable.GetPlayerSkillData(playerClassData.SpecialSkillId);

        PlayerSkillCreateInfo createInfo = new PlayerSkillCreateInfo(_petPartyReader, petStatusEffectReceviers, _statusEffectMaker);

        var basicExecution = _skillRegistry.Create(basicSkillData.ExecutionId, createInfo);
        var specialExecution = _skillRegistry.Create(specialSkillData.ExecutionId, createInfo);

        if(basicExecution != null && specialExecution != null)
        {
            PlayerSkill basicSkill = new PlayerSkill(basicSkillData, basicExecution
                , _playerViewModel, statController);

            PlayerSkill specialSkill = new PlayerSkill(specialSkillData, specialExecution
                , _playerViewModel, statController);

            _build.SetSkill(SkillSlot.Basic, basicSkill);
            _build.SetSkill(SkillSlot.Special, specialSkill);
        }

        // 테스트 삭제하기
        PlayerSkillData ultimateSkillData = GameManager.DataTable.GetPlayerSkillData(playerClassData.UltimateSkillIds[0]);
        Debug.Log($"UltimateSkillData : {ultimateSkillData.Name} / {ultimateSkillData.ExecutionId}");
        var ultimateExecution = _skillRegistry.Create(ultimateSkillData.ExecutionId, createInfo);

        PlayerSkill ultimateSkill = new PlayerSkill(ultimateSkillData, ultimateExecution
                , _playerViewModel, statController);
        _build.SetSkill(SkillSlot.Ultimate, ultimateSkill);

        return _build;
    }

    // 궁극기 선택처럼 처음 생성이 아닌 스킬을 추가로 생성할 때 사용
    public void CreatePlayerSkill(string skillId)
    {
        PlayerSkillData skillData = GameManager.DataTable.GetPlayerSkillData(skillId);

        PlayerSkillCreateInfo createInfo = new PlayerSkillCreateInfo(_petPartyReader, _petStatusEffectReceviers, _statusEffectMaker);

        var execution = _skillRegistry.Create(skillData.ExecutionId, createInfo);

        if (execution != null)
        {
            PlayerSkill playerSkill = new PlayerSkill(skillData, execution, _playerViewModel, _statController);

            _build.SetSkill(SkillSlot.Basic, playerSkill);
        }
    }
}
