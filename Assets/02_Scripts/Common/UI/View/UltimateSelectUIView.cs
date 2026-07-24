using System;
using UnityEngine;

public class UltimateSelectUIView : CardSelectUIView
{
    protected override UIType SelfUIType
    {
        get { return UIType.UltimateSelectUIView; }
    }

    public void SetUltimates(string playerClassId, Action<string> onSelected)
    {
        var classData = GameManager.DataTable.GetPlayerClassData(playerClassId);

        if (classData == null)
        {
            Debug.LogWarning($"플레이어 클래스 데이터를 찾을 수 없습니다: {playerClassId}");
            return;
        }

        if (classData.UltimateSkillIds == null || classData.UltimateSkillIds.Count == 0)
        {
            Debug.LogWarning($"선택 가능한 궁극기가 없습니다: {playerClassId}");
            return;
        }

        SetSlots(classData.UltimateSkillIds, onSelected, null);
    }
}
