using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUiManager : MonoBehaviour
{
    public GameObject modeSelectionPanel;
    public GameObject stageSelectPanel;

    private void Start()
    {
        if (modeSelectionPanel != null)
        {
            modeSelectionPanel.SetActive(true);
        }
        if (stageSelectPanel != null)
        {
            stageSelectPanel.SetActive(false);
        }
    }

    public void ShowStageSelection()
    {
        modeSelectionPanel.SetActive(false);
        stageSelectPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        modeSelectionPanel.SetActive(true);
        stageSelectPanel.SetActive(false);
    }

    public void StartGame(int stageNumber)
    {
        GameManager.Instance.currentStage = stageNumber;
        SceneManager.LoadScene("Main");
    }
}
