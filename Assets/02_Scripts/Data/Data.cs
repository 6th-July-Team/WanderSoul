using System;
using UnityEngine;

[Serializable]
public class BaseData
{
    public string Id;
}

[Serializable]
public class EnemyData : BaseData, ISerializationCallbackReceiver
{
    public string TargetPolicy; // PriortyBased == 기본 타입 // WagonOnly == 케러벤(마차)만 공격 // PlayerOnly == 플레이어만 공격
    public string EnemyAttackType; // 공격의 타입 => Melee == 근접 // Projectile == 투사체발사(원거리) // AreaDelayed == 고정 포대형(예고 후 장판 공격) // Steal == 공격 없이 훔치기 기능만

    [NonSerialized] public TargetPolicy Policy; // 실제 데이터로 받아올 타겟 지정 타입
    [NonSerialized] public EnemyAttackType AttackType; // 실제 데이터로 받아올 공격타입

    public void OnAfterDeserialize()
    {
        if(Enum.TryParse(TargetPolicy, true, out Policy) == false)
        {
            DataLog.EnumParseFailed<TargetPolicy>(nameof(EnemyData), Id, TargetPolicy);
        }

        if(Enum.TryParse(EnemyAttackType, true, out AttackType) == false)
        {
            DataLog.EnumParseFailed<EnemyAttackType>(nameof(EnemyData), Id, EnemyAttackType);
        }
    }

    public void OnBeforeSerialize() { }

    public string Name; // Enemy의 이름
    public string Description; // Enemy에 대한 설명

    public int MaxHp; // 최대 체력

    public float DetectRange; // 몬스터를 기준으로 탐색하는 범위
    public float LeashRange; // 마차를 기준으로 탐색하는 범위

    public int Attack; // 공격력
    public float AttackSpeed; // 공격 속도
    public float AttackRange; // 공격 범위

    public float SoulDropChance; // 경헙치 드롭 확률
    public int SoulDropAmount; // 경헙치 드롭량

    public float ExpDropChance; // 경험치 드롭 확률
    public int ExpDropAmount; // 경험치 드롭량

    public float PreferredDistance; // [저격형 전용, 다른 타입의 경우 0으로 할 것] 저격형 몬스터에서 도망치기 위한 범위

    public bool CanMove; // 고정형인지 아닌지
    public float MoveSpeed; // 이동속도
}
