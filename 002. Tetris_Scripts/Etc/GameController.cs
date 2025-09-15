using UnityEngine;

public class GameController : MonoBehaviour
{
    void Start()
    {
        // 게임 시작 시 게임 모드 출력
        GameMode currentMode = GameManager.Instance.GetGameMode();
        Debug.Log("Current Game Mode: " + currentMode);

        // 게임 모드에 맞게 게임을 초기화하거나 실행
        if (currentMode == GameMode.Classic)
        {
            // 클래식 모드에 맞는 로직 처리
            Debug.Log("Starting Classic Mode");
        }
        else if (currentMode == GameMode.Stage)
        {
            // 스테이지 모드에 맞는 로직 처리
            Debug.Log("Starting Stage Mode");
        }
    }
}
