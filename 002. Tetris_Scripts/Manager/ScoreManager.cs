using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;

    private int score = 0;
    public int level = 1;
    private int linesCleared = 0;

    public Board board;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        UpdateUI();
    }

    public void AddScore(int lines)
    {
        int points = 0;
        switch (lines)
        {
            case 1: points = 100; break;
            case 2: points = 300; break;
            case 3: points = 600; break;
            case 4: points = 1000; break;
        }

        score += points;
        linesCleared += lines;

        if (linesCleared >= level * 10)
        {
            level++;
            linesCleared = 0;
            UpdatePieceDropSpeed();
        }

        UpdateUI();
    }

    private void UpdatePieceDropSpeed()
    {
        if (board.activePiece != null)
        {
            float newStepDelay = Mathf.Max(0.1f, 1f - level * 0.05f);
            board.activePiece.stepDelay = newStepDelay;
        }
    }

    private void UpdateUI()
    {
        scoreText.text = $"Score: {score}";
        levelText.text = $"Level: {level}";
    }
}
