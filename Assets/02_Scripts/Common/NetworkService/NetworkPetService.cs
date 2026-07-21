



public class NetworkPetService
{
    public PetViewModel CreatePetViewModel(string petId)
    {
        var petBaseData = GameManager.DataTable.GetPetStatData(petId);

        PetModel model = new();
        model.HP = petBaseData.MaxHealth;
        model.MaxHp = petBaseData.MaxHealth;

        PetViewModel viewModel = new(model);

        return viewModel;
    }
}
