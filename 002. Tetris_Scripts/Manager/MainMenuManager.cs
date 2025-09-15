using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject stageSelectionPanel;  // �������� ���� �г�
    public Button playButton;
    public Button nextStageButton;
    public Button prevStageButton;
    public TMP_Text stageText;

    private int currentStage = 1;
    private const int minStage = 1;
    private const int maxStage = 10;

    void Awake()
    {
        // �̱��� �ʱ�ȭ
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // ���� �ν��Ͻ��� ������ ����
        }
    }

    void Start()
    {
        // �������� ���� �г� �ʱ�ȭ
        if (stageSelectionPanel != null)
        {
            stageSelectionPanel.SetActive(false); // ó������ �������� ���� �г� �����
        }

        if (nextStageButton != null)
        {
            nextStageButton.onClick.AddListener(() => ChangeStage(1)); // ���� �������� ��ư
        }

        if (prevStageButton != null)
        {
            prevStageButton.onClick.AddListener(() => ChangeStage(-1)); // ���� �������� ��ư
        }

        if (playButton != null)
        {
            playButton.onClick.AddListener(PlayStage); // �������� ���� ��ư
        }

        UpdateStageText();
    }

    void OnEnable()
    {
        // �� ��ȯ �� stageSelectionPanel ���� �ʱ�ȭ
        if (stageSelectionPanel != null)
        {
            stageSelectionPanel.SetActive(false); // �������� ���� �г� �����
        }
    }

    // Ŭ���� ��� ����
    public void PlayClassicMode()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetGameMode(GameMode.Classic); // Ŭ���� ��� ����
            SceneManager.LoadScene("Tetris");                 // Tetris �� �ε�
        }
        else
        {
            Debug.LogError("GameManager instance is null!");
        }
    }

    // �������� ��� ����
    public void ShowStageSelection()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetGameMode(GameMode.Stage); // �������� ��� ����
            GameManager.Instance.SetCurrentStage(1);          // �ʱ� �������� ����

            if (stageSelectionPanel != null)
            {
                stageSelectionPanel.SetActive(true);          // �������� ���� �г� Ȱ��ȭ
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
            stageSelectionPanel.SetActive(false); // ���� �޴��� ���ư� �� �г� �����
        }

        // ���� �޴� �� �ε�
        SceneManager.LoadScene("MainMenu");
    }

    // �������� ����
    private void ChangeStage(int change)
    {
        currentStage += change;
        currentStage = Mathf.Clamp(currentStage, minStage, maxStage);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetCurrentStage(currentStage); // GameManager�� ���� �������� ����
        }

        UpdateStageText();
    }

    // �������� �ؽ�Ʈ ������Ʈ
    private void UpdateStageText()
    {
        if (stageText != null)
        {
            stageText.text = $"Stage: {currentStage}";
        }
    }

    // ������ �������� ����
    public void PlayStage()
    {
        if (GameManager.Instance != null)
        {
            SceneManager.LoadScene("Tetris"); // Tetris �� �ε�
        }
        else
        {
            Debug.LogError("GameManager instance is null!");
        }
    }

    // ���� ����
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit!");
    }
}
