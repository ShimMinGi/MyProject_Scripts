using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public FallingBlockSpawn fallingBlockSpawn;

    public int currentStage = 1;
    public bool isStageMode = false;

    private Player _player;
    public Player Player{get{return _player;}}
    
    private void Awake()
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
        
        _player = FindObjectOfType<Player>();   
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Menu")
        {
            _player = FindObjectOfType<Player>();

            if (_player == null)
            {
                Debug.LogError("Player object is missing in the scene!");
            }
        }

        if (scene.name == "Main")
        {
            fallingBlockSpawn = FindObjectOfType<FallingBlockSpawn>();
            MainUiManager uiManager = FindObjectOfType<MainUiManager>();

            if (fallingBlockSpawn == null || uiManager == null)
            {
                return;
            }

            int stageNumber = PlayerPrefs.GetInt("SelectedStage", 1);
            LoadStage(stageNumber);
        }
    }

    public void LoadStage(int stageNumber)
    {
        currentStage = stageNumber;
        StartCoroutine(StartStageCountdown(stageNumber));
    }

    private IEnumerator StartStageCountdown(int stageNumber)
    {
        MainUiManager uiManager = FindObjectOfType<MainUiManager>();
        if (uiManager != null)
        {
            uiManager.countDownText.gameObject.SetActive(true);
            uiManager.countDownText.text = "3";
            yield return new WaitForSeconds(1f);

            uiManager.countDownText.text = "2";
            yield return new WaitForSeconds(1f);

            uiManager.countDownText.text = "1";
            yield return new WaitForSeconds(1f);

            uiManager.countDownText.text = "Go!";
            yield return new WaitForSeconds(0.5f);

            uiManager.countDownText.gameObject.SetActive(false);

            fallingBlockSpawn.ResetBlocks();
            fallingBlockSpawn.SpawnBlock(currentStage);

            uiManager.UpdateStageInfo(currentStage);
        }
    }

    public void OnStageComplete()
    {
        if (isStageMode) // 스테이지 모드가 활성화된 경우
        {
            MainUiManager uiManager = FindObjectOfType<MainUiManager>();
            if (uiManager != null)
            {
                uiManager.ShowClearMessage(); // 클리어 메시지를 띄우고
                uiManager.ShowMenuButton(); // 메뉴 버튼만 활성화
            }
        }
        else
        {
            if (currentStage < 10)
            {
                currentStage++;
                LoadStage(currentStage);
            }
            else
            {
                MainUiManager uiManager = FindObjectOfType<MainUiManager>();
                if (uiManager != null)
                {
                    uiManager.ShowClearMessage();
                }
            }
        }
    }

    public void PlayerDead()
    {
        fallingBlockSpawn.ResetBlocks();
        MainUiManager uiManager = FindObjectOfType<MainUiManager>();
        if (uiManager != null)
        {
            uiManager.ShowRetryAndMenuButtons(); // 재시도와 메뉴 버튼을 활성화
        }
    }

    public void SetStageMode(bool stageMode)
    {
        isStageMode = stageMode; // 스테이지 모드 설정
    }
}
