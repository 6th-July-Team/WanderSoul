

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
        // 맵 진입 시 플레이어를 요청
        return PlayerService.GetPlayerViewModel();
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
