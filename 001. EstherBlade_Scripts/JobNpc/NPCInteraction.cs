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
            Debug.Log("NPC 범위 안에 플레이어 진입");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("NPC 범위 밖으로 나감");
        }
    }

    private void HandleJobSelect()
    {
        if (!isPlayerInRange)
        {
            Debug.Log("NPC 범위 밖이라 직업창을 열 수 없습니다.");
            return;
        }

        bool active = uiManager.OpenJobSelectionUI();
        playerInput.TogglePlayerControl(!active);
    }
}