using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public enum GameMode
{
    Classic,  // 클래식 모드
    Stage     // 스테이지 모드
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject stageSelectionPanel;
    public TMP_Text clearText;

    private GameMode currentGameMode;
    private int currentStage = 1;
    private Board board;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Tetris")
        {
            StartCoroutine(InitializeAfterSceneLoad());
        }
    }

    private IEnumerator InitializeAfterSceneLoad()
    {
        yield return null; // 씬 로드 후 1 프레임 대기

        board = FindObjectOfType<Board>();
        if (board == null)
        {
            Debug.LogError("Board instance not found!");
        }
        else
        {
            Debug.Log("Board instance found.");

            if (currentGameMode == GameMode.Stage)
            {
                board.LoadStage(currentStage);
            }
        }
    }

    public void SetGameMode(GameMode mode)
    {
        currentGameMode = mode;
    }

    public GameMode GetGameMode()
    {
        return currentGameMode;
    }

    public int GetCurrentStage()
    {
        return currentStage;
    }

    public void SetCurrentStage(int stage)
    {
        currentStage = stage;
    }

    public void StageClear()
    {
        if (clearText != null)
        {
            clearText.text = "Clear";
            clearText.gameObject.SetActive(true);
        }

        Debug.Log("Stage Cleared!");
    }
}
