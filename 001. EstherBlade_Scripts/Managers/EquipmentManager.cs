using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public Inventory inventory;
    public CharacterInfoUI characterInfoUI;

    public void Equip(ItemInstance item)
    {
        if (item == null || item.ItemData == null)
        {
            Debug.LogWarning("[Equip] �������� ����ֽ��ϴ�.");
            return;
        }

        characterInfoUI.UpdateSlot(item.ItemData.equipSlot, item);

        // �κ��丮���� ����
        // inventory.RemoveItem(item);
    }

    public void Unequip(EquipSlotType slotType)
    {
        characterInfoUI.UpdateSlot(slotType, null);
    }
}