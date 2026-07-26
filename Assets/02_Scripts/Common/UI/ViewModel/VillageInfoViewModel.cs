using UnityEngine;

public class VillageInfoViewModel : BaseViewModel<VillageModel>
{
    public VillageInfoViewModel(VillageModel model) : base(model)
    {
    }

    public int CurrentReputation
    {
        get { return _model.CurrentReputation; }
    }

    public string TownName
    {
        get
        {
            var townData = GameManager.DataTable.GetTownData(_model.TownDataId);

            if (townData == null)
            {
                return string.Empty;
            }
            return townData.Name;
        }
    }

    public string RegionLogoPath
    {
        get
        {
            var townData = GameManager.DataTable.GetTownData(_model.TownDataId);

            if (townData == null)
            {
                return string.Empty;
            }

            var regionData = GameManager.DataTable.GetRegionData(townData.RegionId);

            if (regionData == null)
            {
                return string.Empty;
            }
            return regionData.EmblemPath;
        }
    }

    public string RegionHudFramePath
    {
        get
        {
            var townData = GameManager.DataTable.GetTownData(_model.TownDataId);

            if (townData == null)
            {
                return string.Empty;
            }

            var regionData = GameManager.DataTable.GetRegionData(townData.RegionId);

            if (regionData == null)
            {
                return string.Empty;
            }
            return regionData.HudFramePath;
        }
    }

    public string ReputationGradeName
    {
        get
        {
            var grade = GetCurrentGrade();

            if (grade == null)
            {
                return string.Empty;
            }
            return grade.Name;
        }
    }

    public string ReputationGradeIconPath
    {
        get
        {
            var grade = GetCurrentGrade();

            if (grade == null)
            {
                return string.Empty;
            }
            return grade.IconPath;
        }
    }

    public float ReputationFillAmount
    {
        get
        {
            var grade = GetCurrentGrade();

            if (grade == null)
            {
                return 0f;
            }

            int min = grade.MinValue;
            int max = grade.MaxValue;
            int range = max - min;

            if (range <= 0)
            {
                return 0f;
            }

            int progress = _model.CurrentReputation - min;
            return (float)progress / range;
        }
    }

    public int VillageLevel
    {
        get { return _model.VillageLevel; }
    }

    private ReputationGradeData GetCurrentGrade()
    {
        return GameManager.DataTable.GetReputationGradeByValue(_model.CurrentReputation);
    }

    public void AddReputation(int amount)
    {
        _model.CurrentReputation += amount;
    }
}
