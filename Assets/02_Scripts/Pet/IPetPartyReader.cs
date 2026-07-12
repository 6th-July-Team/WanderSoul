

public interface IPetPartyReader
{
    int Count { get; }

    PetElement GetPriorityPetElement();
}