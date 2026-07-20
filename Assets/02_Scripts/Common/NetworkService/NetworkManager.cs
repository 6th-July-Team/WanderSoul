

public class NetworkManager
{
    public NetworkPlayerService PlayerService { get; private set; }
    public NetworkWagonService WagonService { get; private set; }

    public void InitNetworkService()
    {
        PlayerService = new(); 
        WagonService = new();
    }

    public void InGameServiceRelease()
    {
        PlayerService.Dispose();
        WagonService.Dispose();
    }

    public void RequestCreatePlayer()
    {
        // 맵 진입 시 플레이어를 요청
        var playerVm = PlayerService.GetPlayerViewModel();

        // 응답 받았다고 가정한다
        OnRecvCreateLocalPlayer(playerVm);
    }

    public void OnRecvCreateLocalPlayer(PlayerViewModel playerVm)
    {
        
    }

    public void RequestCreateWagon(string wagonId)
    {
        var wagonVm = WagonService.GetWagonViewModel(wagonId);

        OnRecvCreateLocalWagon(wagonVm);
    }

    public void OnRecvCreateLocalWagon(WagonViewModel wagonVm)
    {
        
    }
}
