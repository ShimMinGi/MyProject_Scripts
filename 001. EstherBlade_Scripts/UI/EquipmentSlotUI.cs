using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlotUI : MonoBehaviour, IPointerClickHandler
{
    public EquipSlotType slotType;
    public CharacterInfoUI characterInfoUI;
    public Player player;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            player.UnequipItem(slotType);

            characterInfoUI.UpdateSlot(slotType, null);
        }
    }
}