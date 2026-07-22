

public class NetworkManager
{
    public NetworkPlayerService PlayerService { get; private set; } = new();
    public NetworkWagonService WagonService { get; private set; } = new();
    public NetworkEnemyService EnemyService { get; private set; } = new();
    public NetworkPetService PetService { get; private set; } = new();

    public void InGameServiceRelease()
    {
        PlayerService.Dispose();
        WagonService.Dispose();
    }

    public PlayerViewModel RequestCreatePlayer()
    {
        return PlayerService.GetPlayerViewModel();
    }

    public PlayerSkillViewModel RequestPlayerSkillViewModel()
    {
        return PlayerService.GetPlayerSkillViewModel();
    }

    public WagonViewModel RequestCreateWagon()
    {
        return WagonService.GetWagonViewModel();
    }

    public EnemyViewModel CreateEnemyViewModel(string enemyId)
    {
        return EnemyService.CreateEnemyViewModel(enemyId);
    }

    public PetViewModel CreatePetViewModel(string petId)
    {
        return PetService.CreatePetViewModel(petId);
    }
}
