using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public PlayerInputActions InputActions { get; private set; }
    public PlayerInputActions.PlayerActions PlayerActions { get; private set; }
    public PlayerInputActions.UIActions UIActions { get; private set; }

    public event System.Action OnJobSelectRequested; // 외부 구독용 이벤트

    GameManager gameManager;
    UIManager uiManager;

    public CinemachineInputProvider cinemachineInputProvider { get; private set; }

    private void Awake()
    {
        InputActions = new PlayerInputActions();
        PlayerActions = InputActions.Player;

        UIActions = InputActions.UI;
        UIActions.Inventory.performed += OnInventory;

        UIActions.CharacterInfo.performed += OnCharacterInfo;

        UIActions.Interaction.performed += context => OnJobSelectRequested?.Invoke();

        cinemachineInputProvider = FindObjectOfType<CinemachineInputProvider>();
    }

    private void OnCharacterInfo(InputAction.CallbackContext context)
    {
        bool active = uiManager.OpenCharacterInfoUI();
        TogglePlayerControl(!active);
    }


    private void Start()
    {
        gameManager = GameManager.Instance;
        uiManager = gameManager.UIManager;
    }

    private void OnEnable() => InputActions.Enable();
    private void OnDisable() => InputActions.Disable();

    private void OnInventory(InputAction.CallbackContext context)
    {
        bool active = uiManager.OpenInventoryUI();
        TogglePlayerControl(!active);
    }

    public void TogglePlayerControl(bool enable)
    {
        if (enable)
        {
            PlayerActions.Enable();
            cinemachineInputProvider.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            PlayerActions.Disable();
            cinemachineInputProvider.enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}