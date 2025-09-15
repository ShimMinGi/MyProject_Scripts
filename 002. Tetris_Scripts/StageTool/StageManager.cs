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
            Debug.LogError("Tilemap ������Ʈ�� ã�� �� �����ϴ�!");
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
        int maxY = minY + 5;  // �ϴ� 5��

        // ��ֹ��� ������ �������� ��ȣ�� ����

        bool placed = false;
        while (!placed)
        {
            int x = Random.Range(Bounds.xMin, Bounds.xMax);
            int y = Random.Range(minY, maxY);
            Vector3Int tilePosition = new Vector3Int(x, y, 0);

            if (!tilemap.HasTile(tilePosition)) // ���� ��ġ�� ��ֹ��� ���� ���
            {
                tilemap.SetTile(tilePosition, obstacleTile); // ��ֹ� ��ġ
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

        // ���� ��ü ������ ��ȸ�մϴ�.
        for (int x = Bounds.xMin; x < Bounds.xMax; x++)
        {
            for (int y = Bounds.yMin; y < Bounds.yMax; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                // �ش� ��ġ�� Ÿ���� obstacleTile�̸� �����մϴ�.
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

        // ���� ���� �ȿ� �ִ��� Ȯ�� (�ʿ信 ���� �� üũ�� ������ �� �ֽ��ϴ�)
        if (!Bounds.Contains((Vector2Int)tilePosition))
        {
            Debug.LogWarning($"��ǥ {tilePosition}�� ���� ���� ���Դϴ�!");
            return;
        }

        if (!tilemap.HasTile(tilePosition))
        {
            tilemap.SetTile(tilePosition, obstacleTile);
            Debug.Log($"��ǥ {tilePosition}�� ��ֹ��� �����߽��ϴ�.");
        }
        else
        {
            Debug.LogWarning($"��ǥ {tilePosition}���� �̹� Ÿ���� �����մϴ�.");
        }
    }

    public void RemoveTargetObstacle(Vector3Int tilePosition)
    {
        if (tilemap == null || obstacleTile == null)
        {
            Debug.LogError("Tilemap or obstacleTile is not assigned!");
            return;
        }

        // ���� ������ ������� Ȯ��
        if (!Bounds.Contains((Vector2Int)tilePosition))
        {
            Debug.LogWarning($"��ǥ {tilePosition}�� ���� ���� ���Դϴ�!");
            return;
        }

        // �ش� ��ġ�� ��ֹ����� Ȯ�� �� ����
        if (tilemap.GetTile(tilePosition) == obstacleTile)
        {
            tilemap.SetTile(tilePosition, null);
            Debug.Log($"��ǥ {tilePosition}�� ��ֹ��� �����߽��ϴ�.");
        }
        else
        {
            Debug.LogWarning($"��ǥ {tilePosition}���� ��ֹ��� �����ϴ�.");
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
            obstaclePositions = GetObstaclePositions()  // ��ֹ� ��ġ ����
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log($"Stage saved to {path}");
    }

    // ��ֹ� ��ġ �������� �޼��� �߰�
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
            tilemap.ClearAllTiles();  // ���� ��ֹ� ���� �� �ٽ� ��ġ
            foreach (var tilePos in data.obstaclePositions)
            {
                tilemap.SetTile(tilePos, obstacleTile);
            }
        }

        Debug.Log("Stage loaded successfully!");
    }

}
