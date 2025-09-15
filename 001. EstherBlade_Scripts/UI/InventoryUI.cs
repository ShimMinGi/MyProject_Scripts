using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    UIManager uIManager;
    Player player;
    List<ItemSlotUI> itemSlots = new List<ItemSlotUI>();

    [SerializeField] GameObject itemSlotPrefab;
    [SerializeField] Transform contentRoot;

    [SerializeField] Button equipButton;
    [SerializeField] Button useButton;
    [SerializeField] Button dropButton;

    [SerializeField] int initialSlotCount = 30;

    [SerializeField] EquipmentManager equipmentManager;
    [SerializeField] CharacterInfoUI characterInfoUI;


    ItemSlotUI selectedItem;
    GridLayoutGroup gridLayout;

    private void Awake()
    {
        gridLayout = contentRoot.GetComponent<GridLayoutGroup>();
        if (gridLayout != null)
        {
            gridLayout.cellSize = new Vector2(100, 100);
            gridLayout.spacing = new Vector2(10, 10);
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            //gridLayout.constraintCount = 4;
        }

        // 后 浇吩 固府 积己
        for (int i = 0; i < initialSlotCount; i++)
        {
            GameObject go = Instantiate(itemSlotPrefab, contentRoot);
            ItemSlotUI slot = go.GetComponent<ItemSlotUI>();
            slot.Initialize(this);
            slot.SetUI(null);
            itemSlots.Add(slot);
        }
    }

    public void InitializeUI(GameManager gameManager, UIManager uIManager)
    {
        this.uIManager = uIManager;
        player = gameManager.Player;

        gameObject.SetActive(false);

        equipButton.onClick.AddListener(OnEquipButton);
        useButton.onClick.AddListener(OnUseButton);
        dropButton.onClick.AddListener(OnDropButton);
    }

    public void Open()
    {
        selectedItem = null;
        UpdateButtons(null);
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void AddItemSlot(ItemInstance item)
    {
        ItemSlotUI emptySlot = itemSlots.FirstOrDefault(s => s.ItemInstance == null);
        if (emptySlot != null)
        {
            emptySlot.SetUI(item);
        }
        else
        {
            GameObject go = Instantiate(itemSlotPrefab, contentRoot);
            ItemSlotUI slot = go.GetComponent<ItemSlotUI>();
            slot.Initialize(this);
            slot.SetUI(item);
            itemSlots.Add(slot);
        }
    }

    public void UpdateItemSlot(ItemInstance item)
    {
        ItemSlotUI slot = itemSlots.FirstOrDefault(s => s.ItemInstance == item);
        if (slot == null) return;
        slot.SetUI(item);
    }

    public void SelectItem(ItemSlotUI slot)
    {
        selectedItem = slot;
        UpdateButtons(slot.ItemInstance);
    }

    public void UpdateButtons(ItemInstance item)
    {
        if (item == null)
        {
            useButton.interactable = false;
            equipButton.interactable = false;
            dropButton.interactable = false;
            return;
        }

        switch (item.ItemData.type)
        {
            case ItemType.Consumable:
                useButton.interactable = true;
                equipButton.interactable = false;
                break;
            case ItemType.Equipable:
                useButton.interactable = false;
                equipButton.interactable = true;
                break;
        }
        dropButton.interactable = true;
    }

    public void OnUseButton()
    {
        if (selectedItem == null) return;

        ItemData itemData = selectedItem.ItemInstance.ItemData;
        for (int i = 0; i < itemData.consumables.Length; i++)
        {
            switch (itemData.consumables[i].type)
            {
                case ConsumableType.Health:
                    player.HealthSystem.TakeDamage(-itemData.consumables[i].value);
                    break;
            }
        }

        if (!selectedItem.ItemInstance.Use())
        {
            selectedItem.SetUI(null);
            selectedItem = null;
        }
        else
        {
            selectedItem.SetUI(selectedItem.ItemInstance);
        }

        var selectedSlot = InventorySlotUI.GetCurrentSelected();
        if (selectedSlot != null)
            selectedSlot.Deselect();
    }


    public void OnEquipButton()
    {
        if (selectedItem == null || selectedItem.ItemInstance == null)
            return;

        player.EquipItem(selectedItem.ItemInstance);

        characterInfoUI.UpdateSlot(
            selectedItem.ItemInstance.ItemData.equipSlot,
            selectedItem.ItemInstance
        );

        var selectedSlot = InventorySlotUI.GetCurrentSelected();
        if (selectedSlot != null)
            selectedSlot.Deselect();

        selectedItem.ItemInstance.equipped = true;
        selectedItem.SetUI(selectedItem.ItemInstance);

    }

    public void OnUnequipButton()
    {
        if (selectedItem == null || selectedItem.ItemInstance == null)
            return;

        player.UnequipItem(selectedItem.ItemInstance.ItemData.equipSlot);

        characterInfoUI.UpdateSlot(selectedItem.ItemInstance.ItemData.equipSlot, null);

        var selectedSlot = InventorySlotUI.GetCurrentSelected();
        if (selectedSlot != null)
            selectedSlot.Deselect();

        selectedItem.ItemInstance.equipped = false;
        selectedItem.SetUI(selectedItem.ItemInstance);

    }

    //public void OnEquipButton()
    //{
    //    if (selectedItem == null) return;
    //    player.EquipItem(selectedItem.ItemInstance);

    //    var selectedSlot = InventorySlotUI.GetCurrentSelected();
    //    if (selectedSlot != null)
    //        selectedSlot.Deselect();
    //}

    public void OnDropButton()
    {
        if (selectedItem == null) return;
        if (selectedItem.ItemInstance.equipped)
            return;

        Drop(selectedItem.ItemInstance);
        player.Inventory.RemoveItem(selectedItem.ItemInstance);
        selectedItem.SetUI(null);

        var selectedSlot = InventorySlotUI.GetCurrentSelected();
        if (selectedSlot != null)
            selectedSlot.Deselect();
    }

    void Drop(ItemInstance itemInstance)
    {
        ItemData itemData = itemInstance.ItemData;
        Vector3 dropPosition = player.transform.position + player.transform.forward * 1.5f + player.transform.up * 1.5f;

        GameObject go = Instantiate(itemData.dropPrefab, dropPosition, Quaternion.identity);
        Rigidbody rigidbody = go.GetComponent<Rigidbody>();
        rigidbody.AddForce(player.transform.forward * 2, ForceMode.Impulse);

        ItemObject itemObject = go.GetComponent<ItemObject>();
        itemObject.amount = itemInstance.amount;
    }
}