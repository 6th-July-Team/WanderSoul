
public enum ConvoyFailReason
{
    None,
    PlayerDefeated,
    WagonDestroyed,
    OutOfWagonArea,   
}
public class ConvoyResultModel

{
    public bool IsSuccess;
    public ConvoyFailReason FailReason;

    public float ClearTime;
    public bool IsNewRecord;
    public int KilledMonsterCount;
    public int GainedSoul;
    public int GoldReward;
    public int ReputationReward;

    public int ReputationPenalty;
    public int RepairCost;
    public bool IsRepairCostPaid;
    public int ExtraReputationPenalty;

    public string ReturnTownId;
}
