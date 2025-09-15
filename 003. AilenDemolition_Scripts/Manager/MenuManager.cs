using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject modeSelectionPanel;
    public GameObject stageSelectPanel;

    public Button stageModeButton;
    public Button infinityModeButton;
    public Button backButtonFromStageSelect;

    public Button[] stageButtons;

    void Start()
    {
        stageModeButton.onClick.AddListener(OnStageModeSelected);
        infinityModeButton.onClick.AddListener(OnInfinityModeSelected);

        for (int i = 0; i < stageButtons.Length; i++)
        {
            int stageNumber = i + 1;
            stageButtons[i].onClick.AddListener(() => LoadStage(stageNumber));
            GameManager.Instance.isStageMode = true;
        }

        backButtonFromStageSelect.onClick.AddListener(OnBackFromStageSelect);

        stageSelectPanel.SetActive(false);
    }

    void OnStageModeSelected()
    {
        modeSelectionPanel.SetActive(false);
        stageSelectPanel.SetActive(true);
    }

    void OnInfinityModeSelected()
    {
        PlayerPrefs.SetInt("SelectedStage", 1);
        SceneManager.LoadScene("Main");
        GameManager.Instance.isStageMode = false;
        Debug.Log("Infinity Mode Selected");
    }

    void OnBackFromStageSelect()
    {
        stageSelectPanel.SetActive(false);
        modeSelectionPanel.SetActive(true);
    }

    void LoadStage(int stageNumber)
    {
        PlayerPrefs.SetInt("SelectedStage", stageNumber);
        SceneManager.LoadScene("Main");
        Debug.Log("Loading Main Scene with Stage " + stageNumber);
    }
}
