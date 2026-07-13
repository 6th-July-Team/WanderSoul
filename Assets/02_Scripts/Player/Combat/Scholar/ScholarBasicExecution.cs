using System.Collections.Generic;

public class ScholarBasicExecution : ISkillExecution
{
    private readonly IPetPartyReader _petPartyReader;
    private readonly Dictionary<PetElement, IElementArrowVariant> _variants;

    public ScholarBasicExecution(IPetPartyReader petPartyReader)
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

    public bool CanExecute(SkillUseContext context)
    {
        return context.AimDirection.sqrMagnitude > 0.001f;
    }

    public void Execute(SkillUseContext context, float damage)
    {
        PetElement element = _petPartyReader.GetPriorityPetElement();
        _variants[element].Fire(context, damage);
    }
}