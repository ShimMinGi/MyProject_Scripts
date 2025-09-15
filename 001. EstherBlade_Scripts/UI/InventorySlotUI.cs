using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject highlight;
    private static InventorySlotUI currentSelected;

    void Awake()
    {
        SetHighlight(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentSelected != null && currentSelected != this)
        {
            currentSelected.SetHighlight(false);
        }

        bool isSelected = currentSelected == this;
        if (isSelected)
        {
            Deselect();
        }
        else
        {
            SetHighlight(true);
            currentSelected = this;
        }
    }

    public void SetHighlight(bool active)
    {
        if (highlight != null)
            highlight.SetActive(active);
    }

    public void Deselect()
    {
        SetHighlight(false);
        if (currentSelected == this)
            currentSelected = null;
    }

    public static InventorySlotUI GetCurrentSelected()
    {
        return currentSelected;
    }
}