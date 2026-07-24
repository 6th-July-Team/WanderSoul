using System.Collections.Generic;

public class NetworkPetService
{
    private readonly List<PetViewModel> _petViewModels = new();

    public IReadOnlyList<PetViewModel> GetPetViewModels(List<string> petIds)
    {
        if (_petViewModels.Count == 0)
        {
            CreatePetViewModels(petIds);
        }

        return _petViewModels;
    }

    private void CreatePetViewModels(List<string> petIds)
    {
        foreach (var petId in petIds)
        {
            _petViewModels.Add(CreatePetViewModel(petId));
        }
    }

    private PetViewModel CreatePetViewModel(string petId)
    {
        var petBaseData = GameManager.DataTable.GetPetStatData(petId);

        PetModel model = new();
        model.HP = petBaseData.MaxHealth;
        model.MaxHp = petBaseData.MaxHealth;

        return new PetViewModel(model);
    }

    public void Dispose()
    {
        foreach (var viewModel in _petViewModels)
        {
            viewModel.Dispose();
        }

        _petViewModels.Clear();
    }
}
