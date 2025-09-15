using Unity.VisualScripting;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    public QuestData currentQuest;
    public QuestUI questUI;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartQuest(QuestData quest)
    {
        currentQuest = quest;
        currentQuest.questState = QuestState.InProgress;
        Debug.Log($"Äù½ºÆ® ½ÃÀÛ: {quest.questName}");
    }

    public void CheckMovement(string direction)
    {
        if (currentQuest != null && currentQuest.questState == QuestState.InProgress)
        {
            currentQuest.CheckCompletion(direction);
        }
    }

    public void OpenQuestUI()
    {
        if (questUI != null)
        {
            questUI.Open();
        }
    }

    public void CloseQuestUI()
    {
        if (questUI != null)
        {
            questUI.Close();
        }
    }
}