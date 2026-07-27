using System;
using System.Collections.Generic;

public class PlayerOutGameViewModel : BaseViewModel<PlayerOutGameModel>
{
    public event Action<int> OnLevelUp;

    public PlayerOutGameViewModel(PlayerOutGameModel model) : base(model)
    {
    }

    // PlayerLevelData에 없는 레벨에 도달했을 때 쓰는 폴백 값
    private const float BASE_REQUIRED_EXP = 30f;
    private const float REQUIRED_EXP_PER_LEVEL = 15f;

    public int GetSoul => _model.Soul;
    public int GetGold => _model.Gold;
    public float GetExp => _model.Exp;
    public int GetLevel => _model.Level;
    public float GetRequiredExp => GetRequiredExpByLevel(_model.Level);
    public int GetReputation => _model.Reputation;

    public void AddSoul(int soulAmount)
    {
        _model.Soul += soulAmount;
    }

    public void ReduceSoul(int soulAmount)
    {
        _model.Soul -= soulAmount;
    }

    public void AddGold(int goldAmount)
    {
        _model.Gold += goldAmount;
    }

    public void ReduceGold(int goldAmount)
    {
        _model.Gold -= goldAmount;
    }

    public void AddExp(float expAmount)
    {
        _model.Exp += expAmount;

        int maxLevel = GameManager.DataTable.GetMaxPlayerLevel();

        while (IsMaxLevel(maxLevel) == false && _model.Exp >= GetRequiredExpByLevel(_model.Level))
        {
            _model.Exp -= GetRequiredExpByLevel(_model.Level);
            _model.Level++;

            OnLevelUp?.Invoke(_model.Level);
        }

        if (IsMaxLevel(maxLevel) == false)
        {
            return;
        }

        float requiredExp = GetRequiredExpByLevel(_model.Level);

        if (_model.Exp > requiredExp)
        {
            _model.Exp = requiredExp;
        }
    }

    private bool IsMaxLevel(int maxLevel)
    {
        if (maxLevel <= 0)
        {
            return false;
        }

        return _model.Level >= maxLevel;
    }

    private float GetRequiredExpByLevel(int level)
    {
        var levelData = GameManager.DataTable.GetPlayerLevelData(level);

        if (levelData == null || levelData.RequiredExp <= 0)
        {
            return BASE_REQUIRED_EXP + (level - 1) * REQUIRED_EXP_PER_LEVEL;
        }

        return levelData.RequiredExp;
    }

    public IReadOnlyDictionary<string, int> GetLevelUpPicks => _model.LevelUpPicks;

    public void AddLevelUpPick(string optionId)
    {
        _model.AddLevelUpPick(optionId);
    }

    public string GetSelectedUltimateSkillId => _model.SelectedUltimateSkillId;

    public void SetSelectedUltimateSkillId(string skillId)
    {
        _model.SelectedUltimateSkillId = skillId;
    }

    public void ResetLevel()
    {
        _model.ClearLevelUpPicks();
        _model.SelectedUltimateSkillId = null;
        _model.Level = 1;
        _model.Exp = 0f;
    }

    public void AddReputation(int reputationAmount)
    {
        _model.Reputation += reputationAmount;
    }
}
