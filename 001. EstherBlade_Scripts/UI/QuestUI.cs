using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class QuestUI : MonoBehaviour
{
    public GameObject questPanel;

    public TMP_Text acceptButtonText;

    public QuestData selectedQuest;
    public List<QuestData> availableQuests = new List<QuestData>();

    public Transform questListContainer;
    public GameObject questItemPrefab;

    private void Start()
    {
        questPanel.SetActive(false);

        // �׽�Ʈ�� ����Ʈ �߰�
        availableQuests.Add(new QuestData { questName = "�·� �̵�", movementDirection = "left" });
        availableQuests.Add(new QuestData { questName = "��� �̵�", movementDirection = "right" });

        acceptButtonText.text = "����";
    }

    public void ToggleQuestUI()
    {
        questPanel.SetActive(!questPanel.activeSelf);
    }

    public void UpdateQuestList()
    {
        foreach (Transform child in questListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (QuestData quest in availableQuests)
        {
            GameObject questItem = Instantiate(questItemPrefab, questListContainer);
            TMP_Text questText = questItem.GetComponentInChildren<TMP_Text>();
            questText.text = quest.questName;

            questItem.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => SelectQuest(quest));
        }
    }

    public void SelectQuest(QuestData quest)
    {
        selectedQuest = quest;
        acceptButtonText.text = $"{quest.questName} �����ϱ�";
    }

    public void AcceptQuest()
    {
        if (selectedQuest != null)
        {
            QuestManager.Instance.StartQuest(selectedQuest);
            ToggleQuestUI();
        }
    }

    public void Open()
    {
        questPanel.SetActive(true);
        UpdateQuestList();
    }

    public void Close()
    {
        questPanel.SetActive(false);
    }
}