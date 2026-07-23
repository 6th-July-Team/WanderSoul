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
            _elementSkillData = GameManager.DataTable.GetPlayerSkillData(SkillData.Id + "_" + _element.ToString());
        }


        Vector3 skillCenter = context.AimWorldPoint;
        skillCenter.y += 0f;

        SearchUtil.FindNearestSphere(skillCenter, _elementSkillData.Radius, LayerMask.GetMask("Enemy"), _targets);

        DamageInfo damageInfo = new DamageInfo(damage, context.AimDirection, Utils.GetTypeByPetElement(_element));

        foreach (var target in _targets)
        {
            if (target == null)
                continue;

            target.GetComponent<IDamageable>().TakeDamage(damageInfo);
        }







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
