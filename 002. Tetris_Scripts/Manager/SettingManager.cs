using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public ResolutionManager resolutionManager;
    public KeyBindManager keyBindManager;
    public Button applyButton;
    public Button closeButton;

    public static SettingsManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneChange;
        }
        else
        {
            Destroy(gameObject);
        }

        // 초기 설정 불러오기
        LoadSettings();
    }

    private void Start()
    {
        if (EventSystem.current == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            DontDestroyOnLoad(eventSystem);
        }

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        if (applyButton != null)
        {
            applyButton.onClick.AddListener(ApplySettings);
        }

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseSettingsPanel);
        }
    }

    public void OpenSettingsPanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
        Time.timeScale = 0f;
    }

    public void CloseSettingsPanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        Time.timeScale = 1f;
    }

    public void ApplySettings()
    {
        if (resolutionManager != null)
        {
            resolutionManager.ApplyResolution();
        }

        if (keyBindManager != null)
        {
            keyBindManager.ApplyKeyBinds();
        }

        SaveSettings();
        CloseSettingsPanel();
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("resolutionWidth") && PlayerPrefs.HasKey("resolutionHeight"))
        {
            SettingsData.Instance.resolutionWidth = PlayerPrefs.GetInt("resolutionWidth");
            SettingsData.Instance.resolutionHeight = PlayerPrefs.GetInt("resolutionHeight");
        }

        SettingsData.Instance.moveLeftKey = LoadKey("moveLeftKey", KeyCode.A);
        SettingsData.Instance.moveRightKey = LoadKey("moveRightKey", KeyCode.D);
        SettingsData.Instance.moveDownKey = LoadKey("moveDownKey", KeyCode.S);
        SettingsData.Instance.rotateLeftKey = LoadKey("rotateLeftKey", KeyCode.Q);
        SettingsData.Instance.rotateRightKey = LoadKey("rotateRightKey", KeyCode.E);
        SettingsData.Instance.hardDropKey = LoadKey("hardDropKey", KeyCode.Space);

        Screen.SetResolution(SettingsData.Instance.resolutionWidth, SettingsData.Instance.resolutionHeight, Screen.fullScreen);

        if (keyBindManager != null)
        {
            keyBindManager.LoadKeyBindings();
        }

        Debug.Log($"Loaded Key Bindings: {SettingsData.Instance.moveLeftKey}, {SettingsData.Instance.moveRightKey}, {SettingsData.Instance.moveDownKey}, {SettingsData.Instance.rotateLeftKey}, {SettingsData.Instance.rotateRightKey}, {SettingsData.Instance.hardDropKey}");
    }

    private KeyCode LoadKey(string keyName, KeyCode defaultKey)
    {
        if (PlayerPrefs.HasKey(keyName))
        {
            string keyString = PlayerPrefs.GetString(keyName);
            if (System.Enum.TryParse(keyString, out KeyCode key))
            {
                return key;
            }
        }
        return defaultKey;
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetInt("resolutionWidth", SettingsData.Instance.resolutionWidth);
        PlayerPrefs.SetInt("resolutionHeight", SettingsData.Instance.resolutionHeight);
        PlayerPrefs.SetString("moveLeftKey", SettingsData.Instance.moveLeftKey.ToString());
        PlayerPrefs.SetString("moveRightKey", SettingsData.Instance.moveRightKey.ToString());
        PlayerPrefs.SetString("moveDownKey", SettingsData.Instance.moveDownKey.ToString());
        PlayerPrefs.SetString("rotateLeftKey", SettingsData.Instance.rotateLeftKey.ToString());
        PlayerPrefs.SetString("rotateRightKey", SettingsData.Instance.rotateRightKey.ToString());
        PlayerPrefs.SetString("hardDropKey", SettingsData.Instance.hardDropKey.ToString());

        PlayerPrefs.Save();
        Debug.Log("Settings Saved");
    }

    private void OnSceneChange(Scene scene, LoadSceneMode mode)
    {
        settingsPanel = GameObject.Find("SettingsPanel");
        applyButton = GameObject.Find("ApplyButton").GetComponent<Button>();
        closeButton = GameObject.Find("CloseButton").GetComponent<Button>();

        keyBindManager = FindObjectOfType<KeyBindManager>(); // 새로운 씬에서도 키바인드 매니저를 가져옴

        if (settingsPanel != null)
        {
            GameObject buttonObject = GameObject.Find("SettingsButton");
            if (buttonObject != null)
            {
                buttonObject.GetComponent<Button>().onClick.AddListener(OpenSettingsPanel);
                Debug.Log("SettingsButton Event 연결됨");
            }
            CloseSettingsPanel();
        }

        if (applyButton != null)
        {
            applyButton.onClick.AddListener(ApplySettings);
            Debug.Log("ApplyButton Event 연결됨");
        }

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseSettingsPanel);
            Debug.Log("CloseButton Event 연결됨");
        }

        LoadSettings();
        Debug.Log("키 값 업데이트 완료.");
    }
}
