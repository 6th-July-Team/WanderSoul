using UnityEngine;

[CreateAssetMenu(fileName = "SOPetBaseStat", menuName = "ScriptableObjects/Pet/SOPetBaseStat")]
public class SOPetBaseStat : ScriptableObject
{
    public float MaxHp;
    public float MoveSpeed;
    // public float AttackPower; // 기본 공격 스킬 마다 공격력 수치가 다르기 때문에 제거
    public float AttackSpeed;
    // public float AttackRange; // 일반 공격, 스킬에 해당하는 데이터 데리븐에서 가져오기
}
