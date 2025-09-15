using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class UIManager : MonoBehaviour
{
    // public Image nextBlockImage; // 기존의 이미지 사용 부분은 주석 처리 또는 삭제

    // **추가된 부분**
    public Tilemap nextBlockTilemap; // Inspector에서 NextBlockDisplay의 Tilemap을 연결

    // 다음 블록을 표시할 타일의 기준 위치 (예: (0, 0))
    public Vector3Int nextBlockPosition = new Vector3Int(0, 0, 0);

    // 다음 블록 표시를 업데이트하는 메서드
    public void UpdateNextBlockDisplay(TetrominoData tetrominoData)
    {
        if (nextBlockTilemap != null && tetrominoData != null)
        {
            // 이전에 그려진 블록 지우기
            nextBlockTilemap.ClearAllTiles();

            // 블록의 셀들을 반복하여 타일맵에 그리기
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
