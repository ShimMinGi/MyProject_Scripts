using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject clearPanel; // Ŭ���� ���� �г� �߰�

    public bool isGameOver = false;

    public void TriggerGameOver()
    {
        gameOverPanel.SetActive(true);
        isGameOver = true;
        Time.timeScale = 0f;
    }

    public void TriggerStageClear()
    {
        clearPanel.SetActive(true); // Ŭ���� ���� �г� ǥ��
        isGameOver = true;
        Time.timeScale = 0f;
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
