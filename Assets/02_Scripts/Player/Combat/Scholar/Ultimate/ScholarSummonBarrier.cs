using UnityEngine;

public class ScholarSummonBarrier : IPlayerSkillExecution, ISkillRangeCheckable
{
    private SkillRangeIndicator _skillRangeIndicator;
    private Collider[] _targets = new Collider[64];


    public void Execute(PlayerSkillUseContext context, PlayerSkillData SkillData, float damage)
    {
        _skillRangeIndicator.Hide();

        Vector3 skillCenter = context.AimWorldPoint;
        skillCenter.y += 3.5f;//SkillData.Radius;

        // TODO(김익환): 결계 소환
        var barrier = Object.Instantiate(Utils.ResourcesLoad<GameObject>("ScholarBarrier"), skillCenter, Quaternion.identity);
        if (barrier.TryGetComponent(out ScholarBarrier scholarBarrier))
        {
            scholarBarrier.Init(SkillData);
        }

        Debug.Log("궁극기 사용");
    }

    public void CheckSkillRange(PlayerSkillUseContext context, PlayerSkillData SkillData)
    {
        InitIndicator();

        // TODO 데이터 작성 후 주석 해제
        _skillRangeIndicator.Show(context.AimWorldPoint, 7f/*SkillData.Radius*/);
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
