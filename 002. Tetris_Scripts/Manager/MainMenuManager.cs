using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject stageSelectionPanel;  // 스테이지 선택 패널
    public Button playButton;
    public Button nextStageButton;
    public Button prevStageButton;
    public TMP_Text stageText;

    private int currentStage = 1;
    private const int minStage = 1;
    private const int maxStage = 10;

    void Awake()
    {
        // 싱글턴 초기화
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 기존 인스턴스가 있으면 삭제
        }
    }

    void Start()
    {
        // 스테이지 선택 패널 초기화
        if (stageSelectionPanel != null)
        {
            stageSelectionPanel.SetActive(false); // 처음에는 스테이지 선택 패널 숨기기
        }

        if (nextStageButton != null)
        {
            nextStageButton.onClick.AddListener(() => ChangeStage(1)); // 다음 스테이지 버튼
        }

        if (prevStageButton != null)
        {
            prevStageButton.onClick.AddListener(() => ChangeStage(-1)); // 이전 스테이지 버튼
        }

        if (playButton != null)
        {
            playButton.onClick.AddListener(PlayStage); // 스테이지 시작 버튼
        }

        UpdateStageText();
    }

    void OnEnable()
    {
        // 씬 전환 후 stageSelectionPanel 상태 초기화
        if (stageSelectionPanel != null)
        {
            stageSelectionPanel.SetActive(false); // 스테이지 선택 패널 숨기기
        }
    }

    // 클래식 모드 선택
    public void PlayClassicMode()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetGameMode(GameMode.Classic); // 클래식 모드 설정
            SceneManager.LoadScene("Tetris");                 // Tetris 씬 로드
        }
        else
        {
            Debug.LogError("GameManager instance is null!");
        }
    }

    // 스테이지 모드 선택
    public void ShowStageSelection()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetGameMode(GameMode.Stage); // 스테이지 모드 설정
            GameManager.Instance.SetCurrentStage(1);          // 초기 스테이지 설정

            if (stageSelectionPanel != null)
            {
                stageSelectionPanel.SetActive(true);          // 스테이지 선택 패널 활성화
            }
        }
        else
        {
            Debug.LogError("GameManager instance is null!");
        }
    }

    public void ReturnToMainMenu()
    {
        if (stageSelectionPanel != null)
        {
            stageSelectionPanel.SetActive(false); // 메인 메뉴로 돌아갈 때 패널 숨기기
        }

        // 메인 메뉴 씬 로드
        SceneManager.LoadScene("MainMenu");
    }

    // 스테이지 변경
    private void ChangeStage(int change)
    {
        currentStage += change;
        currentStage = Mathf.Clamp(currentStage, minStage, maxStage);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetCurrentStage(currentStage); // GameManager에 현재 스테이지 저장
        }

        UpdateStageText();
    }

    // 스테이지 텍스트 업데이트
    private void UpdateStageText()
    {
        if (stageText != null)
        {
            stageText.text = $"Stage: {currentStage}";
        }
    }

    // 선택한 스테이지 시작
    public void PlayStage()
    {
        if (GameManager.Instance != null)
        {
            SceneManager.LoadScene("Tetris"); // Tetris 씬 로드
        }
        else
        {
            Debug.LogError("GameManager instance is null!");
        }
    }

    // 게임 종료
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit!");
    }
}
