using System.Collections.Generic;

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

    public void Execute(PlayerSkillUseContext context, PlayerSkillData SkillData, float damage)
    {
        PetElement element = _petPartyReader.GetPriorityPetElement();
        _variants[element].Fire(context, damage, SkillData);
    }
    public void CheckSkillRange(PlayerSkillUseContext context) { }
}
