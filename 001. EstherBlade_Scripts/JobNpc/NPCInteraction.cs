using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public GameObject talkUI;
    private bool isPlayerInRange = false;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private UIManager uiManager;

    private void Start()
    {
        if (playerInput != null)
            playerInput.OnJobSelectRequested += HandleJobSelect;
    }

    private void OnDestroy()
    {
        if (playerInput != null)
            playerInput.OnJobSelectRequested -= HandleJobSelect;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("NPC ���� �ȿ� �÷��̾� ����");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("NPC ���� ������ ����");
        }
    }

    private void HandleJobSelect()
    {
        if (!isPlayerInRange)
        {
            Debug.Log("NPC ���� ���̶� ����â�� �� �� �����ϴ�.");
            return;
        }

        bool active = uiManager.OpenJobSelectionUI();
        playerInput.TogglePlayerControl(!active);
    }
}