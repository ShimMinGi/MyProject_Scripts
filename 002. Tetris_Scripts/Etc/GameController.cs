using UnityEngine;

public class GameController : MonoBehaviour
{
    void Start()
    {
        // ���� ���� �� ���� ��� ���
        GameMode currentMode = GameManager.Instance.GetGameMode();
        Debug.Log("Current Game Mode: " + currentMode);

        // ���� ��忡 �°� ������ �ʱ�ȭ�ϰų� ����
        if (currentMode == GameMode.Classic)
        {
            // Ŭ���� ��忡 �´� ���� ó��
            Debug.Log("Starting Classic Mode");
        }
        else if (currentMode == GameMode.Stage)
        {
            // �������� ��忡 �´� ���� ó��
            Debug.Log("Starting Stage Mode");
        }
    }
}
