using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public Inventory inventory;
    public CharacterInfoUI characterInfoUI;

    public void Equip(ItemInstance item)
    {
        if (item == null || item.ItemData == null)
        {
            Debug.LogWarning("[Equip] 아이템이 비어있습니다.");
            return;
        }

        characterInfoUI.UpdateSlot(item.ItemData.equipSlot, item);

        // 인벤토리에서 제거
        // inventory.RemoveItem(item);
    }

    public void Unequip(EquipSlotType slotType)
    {
        characterInfoUI.UpdateSlot(slotType, null);
    }
}