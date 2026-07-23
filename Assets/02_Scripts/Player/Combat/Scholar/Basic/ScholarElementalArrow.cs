using System.Collections.Generic;

public class ScholarElementalArrow : IPlayerSkillExecution
{
    private IPetPartyReader _petPartyReader;
    private Dictionary<PetElement, IElementArrowVariant> _variants;

    // 캐싱
    private PlayerSkillData _elementSkillData;
    private PetElement _element;

    public ScholarElementalArrow(IPetPartyReader petPartyReader)
    {
        _petPartyReader = petPartyReader;

        _variants = new Dictionary<PetElement, IElementArrowVariant>
        {
            { PetElement.None, new ScholarMagicArrow() },
            { PetElement.Fire, new ScholarFireArrow() },
            { PetElement.Water, new ScholarWaterArrow() },
            { PetElement.Earth, new ScholarEarthArrow() },
            { PetElement.Air, new ScholarAirArrow() }
        };
    }

    public void Execute(PlayerSkillUseContext context, PlayerSkillData SkillData, float damage, float coolTimeReductionOfStat)
    {
        if(_element == default)
        {
            _element = _petPartyReader.GetPriorityPetElement();
        }

        if(_elementSkillData == null)
        {
            _elementSkillData = GameManager.DataTable.GetPlayerSkillData(SkillData.Id + "_" + _element.ToString());
        }

        _variants[_element].Fire(context, damage, _elementSkillData);

        var viewModel = GameManager.Network.RequestPlayerSkillViewModel();

        float coolTime = context.PlayerSkillModifier.GetValue(_elementSkillData.Id, _elementSkillData.GetSkillSlot()
            , SkillValueType.Cooldown, _elementSkillData.Cooldown) - coolTimeReductionOfStat;


        viewModel.UseSkill(_elementSkillData.GetSkillSlot(), coolTime);
    }

    public void CheckSkillRange(PlayerSkillUseContext context) { }
}
