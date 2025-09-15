using UnityEngine;

public enum QuestState { NotStarted, InProgress, Completed }

[System.Serializable]
public class QuestData
{
    public string questName;
    public QuestState questState = QuestState.NotStarted;
    public string movementDirection;

    public void CheckCompletion(string direction)
    {
        if (movementDirection == direction)
        {
            questState = QuestState.Completed;
            Debug.Log($"Äù½ºÆ® ¿Ï·á: {questName}");
        }
    }
}