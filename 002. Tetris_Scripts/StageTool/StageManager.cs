using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageManager : MonoBehaviour
{
    public Tilemap tilemap;

    public Tile obstacleTile;

    public Vector2Int boardSize = new Vector2Int(10, 20);

    [Header("Stage Settings")]
    public int stageNumber = 1;
    public string savePath = "Assets/Stages";

    private GameObject playerInstance;
    private List<GameObject> enemies = new List<GameObject>();
    private List<GameObject> terrains = new List<GameObject>();
    private HashSet<Vector3> terrainPositions = new HashSet<Vector3>();

    public void Awake()
    {
        tilemap = GetComponent<Tilemap>();

        if (tilemap == null)
        {
            Debug.LogError("Tilemap 컴포넌트를 찾을 수 없습니다!");
        }
    }

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }

    public void SpawnRandomObstacle()
    {
        if (tilemap == null || obstacleTile == null)
        {
            Debug.LogError("Tilemap or obstacleTile is not assigned!");
            return;
        }

        int minY = Bounds.yMin;
        int maxY = minY + 5;  // 하단 5줄

        // 장애물의 개수는 스테이지 번호와 같음

        bool placed = false;
        while (!placed)
        {
            int x = Random.Range(Bounds.xMin, Bounds.xMax);
            int y = Random.Range(minY, maxY);
            Vector3Int tilePosition = new Vector3Int(x, y, 0);

            if (!tilemap.HasTile(tilePosition)) // 현재 위치에 장애물이 없는 경우
            {
                tilemap.SetTile(tilePosition, obstacleTile); // 장애물 배치
                placed = true;
            }
        }
    }

    public void RemoveAllObstacle()
    {
        if (tilemap == null || obstacleTile == null)
        {
            Debug.LogError("Tilemap or obstacleTile is not assigned!");
            return;
        }

        // 보드 전체 범위를 순회합니다.
        for (int x = Bounds.xMin; x < Bounds.xMax; x++)
        {
            for (int y = Bounds.yMin; y < Bounds.yMax; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                // 해당 위치의 타일이 obstacleTile이면 제거합니다.
                if (tilemap.GetTile(tilePosition) == obstacleTile)
                {
                    tilemap.SetTile(tilePosition, null);
                }
            }
        }
    }

    public void TargetSpawnObstacle(Vector3Int tilePosition)
    {
        if (tilemap == null || obstacleTile == null)
        {
            Debug.LogError("Tilemap or obstacleTile is not assigned!");
            return;
        }

        // 보드 영역 안에 있는지 확인 (필요에 따라 이 체크는 생략할 수 있습니다)
        if (!Bounds.Contains((Vector2Int)tilePosition))
        {
            Debug.LogWarning($"좌표 {tilePosition}가 보드 영역 밖입니다!");
            return;
        }

        if (!tilemap.HasTile(tilePosition))
        {
            tilemap.SetTile(tilePosition, obstacleTile);
            Debug.Log($"좌표 {tilePosition}에 장애물을 생성했습니다.");
        }
        else
        {
            Debug.LogWarning($"좌표 {tilePosition}에는 이미 타일이 존재합니다.");
        }
    }

    public void RemoveTargetObstacle(Vector3Int tilePosition)
    {
        if (tilemap == null || obstacleTile == null)
        {
            Debug.LogError("Tilemap or obstacleTile is not assigned!");
            return;
        }

        // 보드 영역을 벗어났는지 확인
        if (!Bounds.Contains((Vector2Int)tilePosition))
        {
            Debug.LogWarning($"좌표 {tilePosition}가 보드 영역 밖입니다!");
            return;
        }

        // 해당 위치가 장애물인지 확인 후 제거
        if (tilemap.GetTile(tilePosition) == obstacleTile)
        {
            tilemap.SetTile(tilePosition, null);
            Debug.Log($"좌표 {tilePosition}의 장애물을 제거했습니다.");
        }
        else
        {
            Debug.LogWarning($"좌표 {tilePosition}에는 장애물이 없습니다.");
        }
    }


    public void SaveStage()
    {
        string folderPath = Path.Combine(Application.dataPath, savePath.TrimStart("Assets/".ToCharArray()));
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log($"Created folder: {folderPath}");
        }

        string path = Path.Combine(folderPath, $"Stage_{stageNumber}.json");
        SaveStageToFile(path);
    }

    public void SaveStageAs(string customPath)
    {
        SaveStageToFile(customPath);
    }

    private void SaveStageToFile(string path)
    {
        StageData data = new StageData
        {
            obstaclePositions = GetObstaclePositions()  // 장애물 위치 저장
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log($"Stage saved to {path}");
    }

    // 장애물 위치 가져오는 메서드 추가
    private List<Vector3Int> GetObstaclePositions()
    {
        List<Vector3Int> positions = new List<Vector3Int>();

        if (tilemap != null && obstacleTile != null)
        {
            for (int x = Bounds.xMin; x < Bounds.xMax; x++)
            {
                for (int y = Bounds.yMin; y < Bounds.yMax; y++)
                {
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);
                    if (tilemap.GetTile(tilePosition) == obstacleTile)
                    {
                        positions.Add(tilePosition);
                    }
                }
            }
        }
        return positions;
    }

    public void LoadStage()
    {
        string path = Path.Combine(Application.dataPath, savePath.TrimStart("Assets/".ToCharArray()), $"Stage_{stageNumber}.json");
        if (File.Exists(path))
        {
            LoadStageFromFile(path);
        }
        else
        {
            Debug.LogError($"File not found: {path}");
        }
    }

    public void LoadStageAs(string customPath)
    {
        if (File.Exists(customPath))
        {
            LoadStageFromFile(customPath);
        }
        else
        {
            Debug.LogError($"File not found: {customPath}");
        }
    }

    private void LoadStageFromFile(string path)
    {
        string json = File.ReadAllText(path);
        StageData data = JsonUtility.FromJson<StageData>(json);

        if (tilemap != null)
        {
            tilemap.ClearAllTiles();  // 기존 장애물 제거 후 다시 배치
            foreach (var tilePos in data.obstaclePositions)
            {
                tilemap.SetTile(tilePos, obstacleTile);
            }
        }

        Debug.Log("Stage loaded successfully!");
    }

}
