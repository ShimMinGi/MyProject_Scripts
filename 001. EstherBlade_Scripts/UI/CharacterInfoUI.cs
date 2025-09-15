using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class EquipmentSlotButton
{
    public EquipSlotType slotType;
    public Button button;          // ���� ��ư
    public Image iconImage;        // ������ �̹���
    public Sprite emptySprite;     // �� ���� �̹���
}

public class CharacterInfoUI : MonoBehaviour
{
    public EquipmentSlotButton[] slots;
    public Player player;

    UIManager uIManager;

    public void InitializeUI(GameManager gameManager, UIManager uIManager)
    {
        this.uIManager = uIManager;
        this.player = gameManager.Player;

        gameObject.SetActive(false);

        // ���� ��Ŭ�� �̺�Ʈ ����
        foreach (var slot in slots)
        {
            EventTrigger trigger = slot.button.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
                trigger = slot.button.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) =>
            {
                PointerEventData ped = (PointerEventData)data;
                if (ped.button == PointerEventData.InputButton.Right)
                {
                    player.UnequipItem(slot.slotType);
                    UpdateSlot(slot.slotType, null);
                }
            });

            trigger.triggers.Add(entry);
        }
    }

    public void Open()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void UpdateSlot(EquipSlotType slotType, ItemInstance item)
    {
        foreach (var slot in slots)
        {
            if (slot.slotType == slotType)
            {
                slot.iconImage.sprite = (item != null) ? item.ItemData.icon : slot.emptySprite;
                break;
            }
        }
    }
}