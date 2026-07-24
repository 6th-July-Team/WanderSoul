using UnityEngine;

public class ScholarSummonBarrier : IPlayerSkillExecution, ISkillRangeCheckable
{
    private SkillRangeIndicator _skillRangeIndicator;


    public void Execute(PlayerSkillUseContext context, PlayerSkillData SkillData, float damage, float coolTimeReductionOfStat)
    {
        _skillRangeIndicator.Hide();

        Vector3 skillCenter = context.AimWorldPoint;
        skillCenter.y += SkillData.Radius;

        GameManager.Pool.SpawnFromPool<ScholarBarrier>(SkillData.VFXPath, skillCenter, Quaternion.identity)
            .Init(SkillData);

        var viewModel = GameManager.Network.RequestPlayerSkillViewModel();

        float coolTime = context.PlayerSkillModifier.GetValue(SkillData.Id, SkillData.GetSkillSlot()
            , SkillValueType.Cooldown, SkillData.Cooldown) - coolTimeReductionOfStat;


        viewModel.UseSkill(SkillData.GetSkillSlot(), coolTime);
    }

    public void CheckSkillRange(PlayerSkillUseContext context, PlayerSkillData SkillData)
    {
        InitIndicator();

        _skillRangeIndicator.Show(context.AimWorldPoint, SkillData.Radius);
    }

    public void HideSkillRange()
    {
        if (_skillRangeIndicator == null)
            return;

        _skillRangeIndicator.Hide();
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
