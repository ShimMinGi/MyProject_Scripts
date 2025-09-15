using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

[DefaultExecutionOrder(-1)]
public class Board : MonoBehaviour
{
    [System.Serializable]
    public class StageData
    {
        public List<Vector3Int> obstaclePositions;
    }

    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }
    public Ghost ghostPiece;

    public GameOverManager gameOverManager;
    public ScoreManager scoreManager;

    public TetrominoData[] tetrominoes;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public Vector3Int spawnPosition = new Vector3Int(-1, 8, 0);

    public KeyBindManager keyBindManager;
    public SettingsManager settingsManager;

    public Tile obstacleTile;

    private bool isStageClearChecked = false;

    // **�߰��� ������**
    private TetrominoData nextTetrominoData;   // ���� ����� ������ ����
    public UIManager uiManager;                // UIManager�� ���� (Inspector���� ����)

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponentInChildren<Piece>();

        for (int i = 0; i < tetrominoes.Length; i++)
        {
            tetrominoes[i].Initialize();
        }

        // UIManager�� ã�ų� Inspector���� �Ҵ�
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
        }
    }

    private void Start()
    {
        if (ghostPiece != null)
        {
            ghostPiece.trackingPiece = activePiece;
        }

        if (GameManager.Instance != null && GameManager.Instance.GetGameMode() == GameMode.Stage)
        {
            int currentStage = GameManager.Instance.GetCurrentStage();
            LoadStage(currentStage);
        }

        // ó���� ���� ����� �����ϰ� �����ϰ� UI ������Ʈ
        nextTetrominoData = tetrominoes[Random.Range(0, tetrominoes.Length)];
        UpdateNextBlockUI();

        // ù ��� ��ȯ
        SpawnPiece();
    }

    public void LoadStage(int stageNumber)
    {
        string path = Path.Combine(Application.dataPath, "Stages", $"Stage_{stageNumber}.json");

        if (!File.Exists(path))
        {
            Debug.LogError($"Stage file not found: {path}");
            return;
        }

        string json = File.ReadAllText(path);
        StageData data = JsonUtility.FromJson<StageData>(json);

        if (tilemap != null)
        {
            tilemap.ClearAllTiles();  // ���� ��ֹ� ���� �� ���� ��ġ
            foreach (var tilePos in data.obstaclePositions)
            {
                tilemap.SetTile(tilePos, obstacleTile);
            }
        }

        Debug.Log($"Stage {stageNumber} loaded successfully!");
    }

    public void CheckStageClear()
    {
        bool allClear = true;

        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos) && tilemap.GetTile(pos) == obstacleTile)
            {
                allClear = false;
                break;
            }
        }

        if (allClear)
        {
            Debug.Log("All obstacles cleared. Clearing tiles...");
            tilemap.ClearAllTiles();
            gameOverManager.TriggerStageClear();
            isStageClearChecked = true;
        }
    }

    public void SpawnPiece()
    {
        TetrominoData data;

        // ���� ����� ������ ������ ���� ���
        data = nextTetrominoData;

        // ���ο� ���� ����� �����ϰ� �����ϰ� UI�� ǥ��
        nextTetrominoData = tetrominoes[Random.Range(0, tetrominoes.Length)];
        UpdateNextBlockUI();

        if (activePiece == null)
        {
            GameObject pieceObject = new GameObject("ActivePiece");
            pieceObject.transform.parent = transform;
            activePiece = pieceObject.AddComponent<Piece>();
        }

        activePiece.Initialize(this, spawnPosition, data);

        if (IsValidPosition(activePiece, spawnPosition))
        {
            Set(activePiece);
        }
        else
        {
            Destroy(activePiece.gameObject);
            activePiece = null;
            GameOver();
        }

        if (isStageClearChecked)
        {
            Destroy(activePiece.gameObject);
            activePiece = null;
            CheckStageClear();
        }
    }

    private void UpdateNextBlockUI()
    {
        if (nextTetrominoData != null && uiManager != null)
        {
            uiManager.UpdateNextBlockDisplay(nextTetrominoData);
        }
    }

    public void GameOver()
    {
        tilemap.ClearAllTiles();
        gameOverManager.TriggerGameOver();
    }

    public void Set(Piece piece)
    {
        if (tilemap == null) return;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        if (tilemap == null) return;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        if (tilemap == null) return false;

        RectInt bounds = Bounds;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            if (tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }

        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = Bounds;
        int row = bounds.yMin;

        int linesCleared = 0;

        while (row < bounds.yMax)
        {
            if (IsLineFull(row))
            {
                LineClear(row);
                linesCleared++;
            }
            else
            {
                row++;
            }
        }

        if (linesCleared > 0)
        {
            scoreManager.AddScore(linesCleared);
        }

        if (GameManager.Instance != null && GameManager.Instance.GetGameMode() == GameMode.Stage)
        {
            CheckStageClear();
        }
    }

    public bool IsLineFull(int row)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if (!tilemap.HasTile(position))
            {
                return false;
            }
        }

        return true;
    }

    public void LineClear(int row)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            tilemap.SetTile(position, null);
        }

        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                tilemap.SetTile(position, above);
            }

            row++;
        }
    }
}
