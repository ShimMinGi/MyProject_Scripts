using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class FallingBlockSpawn : MonoBehaviour
{
    public GameObject fallingBlocks;
    public float spawnX = 0f;
    public float spawnY = 10f;
    public List<FallingBlock> spawnedBlocks = new List<FallingBlock>();

    public void SpawnBlock(int stageNumber)
    {
        Vector2 spawnPosition = new Vector2(spawnX, spawnY);
        GameObject go = Instantiate(fallingBlocks, spawnPosition, Quaternion.identity);
        FallingBlock fallingBlock = go.GetComponent<FallingBlock>();

        if (fallingBlock != null)
        {
            fallingBlock.Init(this);
            fallingBlock.InitializeStats(stageNumber);
            spawnedBlocks.Add(fallingBlock);
        }
    }

    public void RemoveBlock(FallingBlock fallingBlock)
    {
        if (spawnedBlocks.Contains(fallingBlock))
        {
            spawnedBlocks.Remove(fallingBlock);
            Destroy(fallingBlock.gameObject);
        }
    }

    public int GetRemainingBlocksCount()
    {
        return spawnedBlocks.Count;
    }

    public void ResetBlocks()
    {
        foreach (FallingBlock block in spawnedBlocks)
        {
            Destroy(block.gameObject);
        }
        spawnedBlocks.Clear();
    }
}
