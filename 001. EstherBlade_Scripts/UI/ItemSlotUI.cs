using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IPointerClickHandler
{
    InventoryUI inventoryUI;
    ItemInstance itemInstance;
    public ItemInstance ItemInstance => itemInstance;

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private GameObject equippedMark;

    public void Initialize(InventoryUI inventoryUI)
    {
        this.inventoryUI = inventoryUI;
    }

    public void SetUI(ItemInstance itemInstance)
    {
        this.itemInstance = itemInstance;

        if (itemInstance == null)
        {
            if (backgroundImage != null)
                backgroundImage.enabled = true;

            if (itemImage != null)
                itemImage.enabled = false;

            if (amountText != null)
                amountText.text = "";

            if (equippedMark != null)
                equippedMark.SetActive(false);
        }
        else
        {
            if (backgroundImage != null)
                backgroundImage.enabled = true;

            if (itemImage != null)
            {
                itemImage.sprite = itemInstance.ItemData.icon;
                itemImage.enabled = true;
            }

            if (amountText != null)
                amountText.text = itemInstance.amount > 1 ? itemInstance.amount.ToString() : "";

            if (equippedMark != null)
                equippedMark.SetActive(itemInstance.equipped);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        inventoryUI.SelectItem(this);
    }
}