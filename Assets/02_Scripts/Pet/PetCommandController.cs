using System.Collections.Generic;
using UnityEngine;

// 현재 펫 명령어 저장 -> 파티 전체 
// 명령 모드 변경
// 명령에 맞는 State 제공



public class PetCommandController
{
    //private Dictionary<EPetCommand, IPetCommandState> _commandStates = new();

    public EPetCommand CurrentMode { get; private set; }
    //public IPetCommandState CurrentCommandState { get; private set; }

    //public PetCommandController()
    //{
    //    _commandStates = new()
    //    {
    //        { EPetCommand.PlayerFollow, new PlayerFollowState() },
    //        //{ },
    //        //{ },
    //    };

    //    SetCommandMode(EPetCommand.PlayerFollow);
    //}

    //public void SetCommandMode(EPetCommand commandMode)
    //{
    //    if (!_commandStates.TryGetValue(commandMode, out var commandState))
    //        return;

    //    CurrentMode = commandMode;
    //    CurrentCommandState = commandState;
    //}

    private readonly Collider[] _searchBuffer;

    private readonly LayerMask _enemyLayerMask;
    private readonly SOPetSearch _searchData;

    public EPetCommand CurrentCommand { get; private set; } = EPetCommand.PlayerFollow;

    public PetCommandController(
        SOPetSearch searchData,
        LayerMask enemyLayerMask,
        int bufferSize = 32)
    {
        _searchData = searchData;
        _enemyLayerMask = enemyLayerMask;
        _searchBuffer = new Collider[bufferSize];
    }

    public void SetCommandMode(EPetCommand command)
    {
        CurrentCommand = command;
    }

    public PetCommandResult Decide(PetController pet, Transform player, Transform cart)
    {
        Vector3 anchorPosition = GetAnchorPosition(pet, player, cart);
        float searchRadius = GetSearchRadius();

        ITargetable target = SearchUtil.FindNearestTarget(
            anchorPosition,
            searchRadius,
            _enemyLayerMask,
            _searchBuffer
        );

        return new PetCommandResult(
            CurrentCommand,
            anchorPosition,
            target
        );
    }

    private Vector3 GetAnchorPosition(
        PetController pet,
        Transform player,
        Transform cart)
    {
        return CurrentCommand switch
        {
            EPetCommand.PlayerFollow => player.position,
            EPetCommand.GuardCart => cart.position,
            EPetCommand.Aggressive => pet.Position,
            _ => player.position
        };
    }

    private float GetSearchRadius()
    {
        return CurrentCommand switch
        {
            EPetCommand.PlayerFollow => _searchData.RangeWhenFollowPlayer,
            EPetCommand.GuardCart => _searchData.RangeWhenGuardCart,
            EPetCommand.Aggressive => _searchData.RangeWhenAggressive,
            _ => _searchData.RangeWhenFollowPlayer
        };
    }
}

public struct PetCommandResult
{
    public EPetCommand Command;
    public Vector3 AnchorPosition;
    public ITargetable Target;

    public PetCommandResult(EPetCommand command, Vector3 anchorPosition, ITargetable target)
    {
        Command = command;
        AnchorPosition = anchorPosition;
        Target = target;
    }
}

public struct PetCommandContext
{
    public float SearchRadius;

    public PetCommandContext(float searchRadius)
    {
        SearchRadius = searchRadius;
    }
}

//public enum EPetMoveIntent
//{
//    None,
//    Stop,
//    MoveToPosition,
//    ChaseTarget,
//    ReturnToAnchor
//}

//public readonly struct PetCommandResult
//{
//    public readonly EPetMoveIntent MoveIntent;
//    public readonly Vector3 Destination;
//    public readonly ITargetable Target;

//    public PetCommandResult(
//        EPetMoveIntent moveIntent,
//        Vector3 destination,
//        ITargetable target)
//    {
//        MoveIntent = moveIntent;
//        Destination = destination;
//        Target = target;
//    }
//}

//public readonly struct PetCommandContext
//{
//    public readonly PetController Pet;
//    public readonly Transform Player;
//    public readonly Transform Cart;
//    public readonly IReadOnlyList<ITargetable> DetectedTargets;

//    public readonly float AttackRange;
//    public readonly float FollowDistance;
//    public readonly float MaxGuardDistance;

//    public PetCommandContext(
//        PetController pet,
//        Transform player,
//        Transform cart,
//        IReadOnlyList<ITargetable> detectedTargets,
//        float attackRange,
//        float followDistance,
//        float maxGuardDistance)
//    {
//        Pet = pet;
//        Player = player;
//        Cart = cart;
//        DetectedTargets = detectedTargets;
//        AttackRange = attackRange;
//        FollowDistance = followDistance;
//        MaxGuardDistance = maxGuardDistance;
//    }
//}