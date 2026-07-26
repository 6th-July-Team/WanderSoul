using UnityEngine;

public class ScholarElementalExplosion : IPlayerSkillExecution, ISkillRangeCheckable
{
    private SkillRangeIndicator _skillRangeIndicator;
    private Collider[] _targets = new Collider[32];

    // 캐싱
    private PlayerSkillData _elementSkillData;
    private PetElement _element;

    public void CheckSkillRange(PlayerSkillUseContext context, PlayerSkillData SkillData)
    {
        InitIndicator();

        _skillRangeIndicator.Show(context.AimWorldPoint, 7f);
    }

    public void HideSkillRange()
    {
        if (_skillRangeIndicator == null)
            return;

        _skillRangeIndicator.Hide();
    }

    public void Execute(PlayerSkillUseContext context, PlayerSkillData SkillData, float damage, float coolTimeReductionOfStat)
    {
        _skillRangeIndicator.Hide();

        if(_element == default)
        {
            _element = GameManager.PetParty.GetPriorityPetElement();
        }

        if(_elementSkillData == null)
        {
            _elementSkillData = GameManager.DataTable.GetPlayerSkillData(SkillData.Id + "_" + _element.ToString().ToLower());
        }


        AreaOfEffect aoeInstance = Object.Instantiate(Utils.ResourcesLoad<AreaOfEffect>("AreaOfEffect")
                , context.AimWorldPoint, Quaternion.identity);

        aoeInstance.Init(new AreaOfEffectStruct
        (
            context.AimWorldPoint, damage, _elementSkillData.Radius, _elementSkillData.VFXRadius
            , _elementSkillData.GetDamageType(), _elementSkillData.GetTargetType()
            , _elementSkillData.VFXPath
        ));


        var viewModel = GameManager.Network.RequestPlayerSkillViewModel();

        float coolTime = context.PlayerSkillModifier.GetValue(_elementSkillData.Id, _elementSkillData.GetSkillSlot()
            , SkillValueType.Cooldown, _elementSkillData.Cooldown) - coolTimeReductionOfStat;


        viewModel.UseSkill(_elementSkillData.GetSkillSlot(), coolTime);
    }

    private void InitIndicator()
    {
        if (_skillRangeIndicator != null)
            return;

        GameObject decal = Object.Instantiate(Utils.ResourcesLoad<GameObject>("Decal"));
        _skillRangeIndicator = decal.GetComponent<SkillRangeIndicator>();

        _skillRangeIndicator.Hide();
    }
}
