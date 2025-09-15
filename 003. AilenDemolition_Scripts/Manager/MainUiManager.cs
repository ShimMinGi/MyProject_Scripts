using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUiManager : MonoBehaviour
{
    public TextMeshProUGUI stageInfo;
    public TextMeshProUGUI playerHealth;
    public TextMeshProUGUI countDownText;
    public TextMeshProUGUI clearText;
    public Button retryButton;
    public Button menuButton;

    public GameObject[] healthSprites; // ��������Ʈ���� �迭�� ����

    Player player;

    private void Start()
    {
        if (retryButton != null)
        {
            retryButton.gameObject.SetActive(false);
            retryButton.onClick.AddListener(Retry);
        }
        if (menuButton != null)
        {
            menuButton.gameObject.SetActive(false);
            menuButton.onClick.AddListener(GoToMenu);
        }
        if (clearText != null)
        {
            clearText.gameObject.SetActive(false);
        }
    }

    public void UpdateStageInfo(int stageNumber)
    {
        stageInfo.text = $"STAGE: {stageNumber}";
    }

    public void UpdatePlayerHealth(int health)
    {
        //playerHealth.text = $"HEALTH: {health}";

        // ��� ��������Ʈ ��Ȱ��ȭ
        foreach (var sprite in healthSprites)
        {
            sprite.SetActive(false);
        }

        // ���� �����ִ� ü�¸�ŭ ��������Ʈ Ȱ��ȭ
        for (int i = 0; i < health; i++)
        {
            healthSprites[i].SetActive(true);
        }
    }
    

    public void ShowClearMessage()
    {
        clearText.text = "Clear!";
        clearText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(true);
    }

    public void ShowMenuButton()
    {
        menuButton.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
    }

    public void ShowRetryAndMenuButtons()
    {
        retryButton.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(true);
    }

    private void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        //player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
    }

    private void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
