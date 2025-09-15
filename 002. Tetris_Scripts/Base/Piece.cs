using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }
    public int rotationIndex { get; private set; }

    public float stepDelay = 1f;
    public float moveDelay = 0.1f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float moveTime;
    private float lockTime;

    private SettingsManager settingsManager;

    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        this.settingsManager = SettingsManager.instance; // 싱글턴으로 접근
        rotationIndex = 0;

        stepTime = Time.time + stepDelay;
        moveTime = Time.time + moveDelay;
        lockTime = 0f;

        if (cells == null)
        {
            cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = (Vector3Int)data.cells[i];
        }
    }


    private void Update()
    {
        // 설정창이 열려 있는 상태라면 입력 무시
        if (settingsManager != null && settingsManager.settingsPanel.activeSelf)
        {
            return;
        }

        if (board.gameOverManager.gameOverPanel.activeSelf)
        {
            return;
        }

        board.Clear(this);
        lockTime += Time.deltaTime;

        HandleMoveInputs();

        if (Time.time > stepTime)
        {
            Step();
        }

        board.Set(this);
    }

    private void HandleMoveInputs()
    {
        if (Input.GetKeyDown(SettingsData.Instance.moveLeftKey))
        {
            Move(Vector2Int.left);
        }
        else if (Input.GetKeyDown(SettingsData.Instance.moveRightKey))
        {
            Move(Vector2Int.right);
        }

        if (Input.GetKeyDown(SettingsData.Instance.moveDownKey))
        {
            Move(Vector2Int.down);
        }

        if (Input.GetKeyDown(SettingsData.Instance.rotateLeftKey))
        {
            Rotate(1);
        }

        if (Input.GetKeyDown(SettingsData.Instance.rotateRightKey))
        {
            Rotate(-1);
        }

        if (Input.GetKeyDown(SettingsData.Instance.hardDropKey))
        {
            HardDrop();
        }
    }

    private void Step()
    {
        stepTime = Time.time + stepDelay;
        Move(Vector2Int.down);

        if (lockTime >= lockDelay)
        {
            Lock();
        }
    }

    private void Lock()
    {
        board.Set(this);
        board.ClearLines();
        board.SpawnPiece();
    }

    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = position + new Vector3Int(translation.x, translation.y, 0);
        bool valid = board.IsValidPosition(this, newPosition);

        if (valid)
        {
            position = newPosition;
            moveTime = Time.time + moveDelay;
            lockTime = 0f;
        }

        return valid;
    }

    private void Rotate(int direction)
    {
        int originalRotation = rotationIndex;
        rotationIndex = Wrap(rotationIndex + direction, 0, 4);
        ApplyRotationMatrix(direction);

        if (!TestWallKicks(rotationIndex, direction))
        {
            rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }
    }

    private void HardDrop()
    {
        while (Move(Vector2Int.down))
        {
            continue;
        }
        Lock();
    }

    private void ApplyRotationMatrix(int direction)
    {
        float[] matrix = Data.RotationMatrix;

        for (int i = 0; i < cells.Length; i++)
        {
            Vector3 cell = cells[i];
            int x = Mathf.RoundToInt(cell.x * matrix[0] * direction + cell.y * matrix[1] * direction);
            int y = Mathf.RoundToInt(cell.x * matrix[2] * direction + cell.y * matrix[3] * direction);
            cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);
        for (int i = 0; i < data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = data.wallKicks[wallKickIndex, i];
            if (Move(translation))
            {
                return true;
            }
        }
        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;
        if (rotationDirection < 0)
        {
            wallKickIndex--;
        }
        return Wrap(wallKickIndex, 0, data.wallKicks.GetLength(0));
    }

    private int Wrap(int input, int min, int max)
    {
        if (input < min)
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }
}
