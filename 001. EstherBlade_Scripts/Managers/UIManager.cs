using UnityEngine;

public class UIManager : MonoBehaviour
{
    GameManager gameManager;
    InventoryUI inventoryUI;
    JobSelectionUI jobSelectionUI;
    CharacterInfoUI characterInfoUI;

    public InventoryUI InventoryUI => inventoryUI;
    public JobSelectionUI JobSelectionUI => jobSelectionUI;
    public CharacterInfoUI CharacterInfoUI => characterInfoUI;

    public void InitializeUI(GameManager gameManager)
    {
        this.gameManager = gameManager;

        inventoryUI = transform.GetComponentInChildren<InventoryUI>();
        inventoryUI.InitializeUI(gameManager, this);

        jobSelectionUI = transform.GetComponentInChildren<JobSelectionUI>(true);
        jobSelectionUI.InitializeUI(gameManager);

        characterInfoUI = transform.GetComponentInChildren<CharacterInfoUI>(true);
        characterInfoUI.InitializeUI(gameManager, this);
    }

    public bool OpenInventoryUI()
    {
        inventoryUI.Open();
        return inventoryUI.gameObject.activeSelf;
    }

    public bool OpenJobSelectionUI()
    {
        jobSelectionUI.Open();
        return jobSelectionUI.gameObject.activeSelf;
    }

    public bool OpenCharacterInfoUI()
    {
        characterInfoUI.Open();
        return characterInfoUI.gameObject.activeSelf;
    }
}