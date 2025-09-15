using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public FallingBlockSpawn fallingBlockSpawn;

    public void StartStage(int stageNumber)
    {
        Debug.Log($"Stage {stageNumber} 시작");

        UpdateBackground(stageNumber);

        // 블록 생성
        if (fallingBlockSpawn != null)
        {
            fallingBlockSpawn.SpawnBlock(stageNumber);
        }
    }

    private void UpdateBackground(int stageNumber)
    {
        // 스테이지 번호에 따라 배경 변경(추후 업데이트)
    }
}
