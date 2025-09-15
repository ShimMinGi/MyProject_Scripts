using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public FallingBlockSpawn fallingBlockSpawn;

    public void StartStage(int stageNumber)
    {
        Debug.Log($"Stage {stageNumber} ����");

        UpdateBackground(stageNumber);

        // ��� ����
        if (fallingBlockSpawn != null)
        {
            fallingBlockSpawn.SpawnBlock(stageNumber);
        }
    }

    private void UpdateBackground(int stageNumber)
    {
        // �������� ��ȣ�� ���� ��� ����(���� ������Ʈ)
    }
}
