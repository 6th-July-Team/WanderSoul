using System.Collections.Generic;
using UnityEngine;

public class ScholarElementalArrow : IPlayerSkillExecution
{
    private readonly IPetPartyReader _petPartyReader;
    private readonly Dictionary<PetElement, IElementArrowVariant> _variants;

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
        PetElement element = _petPartyReader.GetPriorityPetElement();
        var selectedSkillData = GameManager.DataTable.GetPlayerSkillData(SkillData.Id + "_" + element.ToString());

        _variants[element].Fire(context, damage, selectedSkillData);

        var viewModel = GameManager.Network.RequestPlayerSkillViewModel();

        float coolTime = context.PlayerSkillModifier.GetValue(selectedSkillData.Id, selectedSkillData.GetSkillSlot()
            , SkillValueType.Cooldown, selectedSkillData.Cooldown) - coolTimeReductionOfStat;


        viewModel.UseSkill(selectedSkillData.GetSkillSlot(), coolTime);
    }

    public void CheckSkillRange(PlayerSkillUseContext context) { }
}
