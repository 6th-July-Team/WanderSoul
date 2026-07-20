

public class NetworkManager
{
    public NetworkPlayerService PlayerService { get; private set; }
    public NetworkWagonService WagonService { get; private set; }

    public void InitNetworkService()
    {
        PlayerService = new();
        WagonService = new();
    }

    public void RequestCreateLocalPlayer()
    {
        // 맵 진입 시 플레이어를 요청
        var playerVm = PlayerService.GetPlayerViewModel();

        // 응답 받았다고 가정한다
        OnRecvCreateLocalPlayer(playerVm);
    }

    public void OnRecvCreateLocalPlayer(PlayerViewModel playerVm)
    {
        
    }
}
