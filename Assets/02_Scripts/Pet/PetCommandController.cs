using UnityEngine;

public class PetCommandController
{
    public PetCommand CurrentCommand { get; private set; }

    private Collider[] _searchBuffer;
    private LayerMask _enemyLayerMask;
    private SOPetSearch _searchData;

    IPositionProvider _playerAnchor;
    IPositionProvider _wagonPosAnchor;
    IPositionProvider _petAnchor;

    public PetCommandController(IPositionProvider playerAnchor, IPositionProvider wagonPosAnchor, IPositionProvider petAnchor
        , SOPetSearch searchData, int bufferSize = 32)
    {
        _playerAnchor = playerAnchor;
        _wagonPosAnchor = wagonPosAnchor;
        _petAnchor = petAnchor;

        _searchData = searchData; // TODO(김익환): 추후 데이터가 추가되면 id로 데이터 가져오기
        _searchBuffer = new Collider[bufferSize];

        _enemyLayerMask = LayerMask.GetMask("Enemy");

        SetCommandMode(PetCommand.PlayerFollow);
    }

    public void SetCommandMode(PetCommand command)
    {
        Debug.Log($"펫 명령 모드 변경: {CurrentCommand} -> {command}");
        CurrentCommand = command;
    }

    public PetCommandResult GetCommandResult()
    {
        IPositionProvider anchor = GetAnchor();
        float searchRadius = GetSearchRadius("TEMP 이후 펫 ID 할당하기");

        ITargetable target = SearchUtil.FindNearestSphere(anchor.Position, searchRadius, _enemyLayerMask, _searchBuffer);

        return new PetCommandResult(CurrentCommand, anchor, target);
    }

    private IPositionProvider GetAnchor()
    {
        return CurrentCommand switch
        {
            PetCommand.PlayerFollow => _playerAnchor,
            PetCommand.GuardWagon => _wagonPosAnchor,
            PetCommand.Aggressive => _petAnchor,
            _ => throw new System.ArgumentOutOfRangeException("잘못된 명령 모드")
        };
    }

    private float GetSearchRadius(string petId)
    {
        // TODO(김익환): 추후 데이터가 추가되면 petId로 데이터 가져오기
        //var searchRadiusData = GameManager.Instance.GetSearchData(petId);

        return CurrentCommand switch
        {
            PetCommand.PlayerFollow => _searchData.RangeWhenFollowPlayer,
            PetCommand.GuardWagon => _searchData.RangeWhenGuardCart,
            PetCommand.Aggressive => _searchData.RangeWhenAggressive,
            _ => throw new System.ArgumentOutOfRangeException("잘못된 명령 모드")
        };
    }
}

public struct PetCommandResult
{
    public PetCommand Command;
    public IPositionProvider Anchor;
    public ITargetable Target;

    public PetCommandResult(PetCommand command, IPositionProvider anchor, ITargetable target)
    {
        Command = command;
        Anchor = anchor;
        Target = target;
    }
}