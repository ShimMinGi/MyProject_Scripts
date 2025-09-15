using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class UIManager : MonoBehaviour
{
    // public Image nextBlockImage; // ������ �̹��� ��� �κ��� �ּ� ó�� �Ǵ� ����

    // **�߰��� �κ�**
    public Tilemap nextBlockTilemap; // Inspector���� NextBlockDisplay�� Tilemap�� ����

    // ���� ����� ǥ���� Ÿ���� ���� ��ġ (��: (0, 0))
    public Vector3Int nextBlockPosition = new Vector3Int(0, 0, 0);

    // ���� ��� ǥ�ø� ������Ʈ�ϴ� �޼���
    public void UpdateNextBlockDisplay(TetrominoData tetrominoData)
    {
        if (nextBlockTilemap != null && tetrominoData != null)
        {
            // ������ �׷��� ��� �����
            nextBlockTilemap.ClearAllTiles();

            // ����� ������ �ݺ��Ͽ� Ÿ�ϸʿ� �׸���
            foreach (var cell in tetrominoData.cells)
            {
                Vector3Int tilePosition = new Vector3Int(cell.x, cell.y, 0) + nextBlockPosition;
                nextBlockTilemap.SetTile(tilePosition, tetrominoData.tile);
            }
        }
        else
        {
            Debug.LogError("Next block tilemap or Tetromino data is missing.");
        }
    }
}
