using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstance
{
    public ItemData ItemData;
    public int amount;
    public bool equipped;

    public ItemInstance(ItemData itemData)
    {
        this.ItemData = itemData;
        amount = 1;
        equipped = false;
    }

    public bool Use(int value = 1)
    {
        amount -= value;
        if (amount <= 0)
        {
            amount = 0;
            return false;
        }
        return true;
    }
}

public class Inventory : MonoBehaviour
{
    GameManager _gameManager;
    UIManager _uiManager;
    InventoryUI _inventoryUI;
    FloatingTextManager _floatingTextManager;

    List<ItemInstance> items = new List<ItemInstance>();

    private const int MAX_INVENTORY_SIZE = 32;

    public void Initialize(GameManager gameManager)
    {
        _gameManager = gameManager;
        _uiManager = gameManager.UIManager;
        _inventoryUI = _uiManager.InventoryUI;
        _floatingTextManager = gameManager.FloatingTextManager;
    }

    private void CleanUpEmptyItems()
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (items[i].amount <= 0)
            {
                items.RemoveAt(i);
            }
        }
    }

    public bool AddItem(ItemData itemData, int amount = 1)
    {
        CleanUpEmptyItems();

        if (items.Count >= MAX_INVENTORY_SIZE)
        {
            _floatingTextManager.CreateFloatingText("Full Inventory!!!", Vector3.zero);
            return false;
        }

        if (amount == 0)
            return false;

        for (int i = 0; i < items.Count; i++)
        {
            ItemInstance item = items[i];

            if (item.ItemData == itemData && itemData.canStack && itemData.maxStackAmount > item.amount)
            {
                int diff = Mathf.Min(amount, itemData.maxStackAmount - item.amount);
                amount -= diff;
                item.amount += diff;

                _inventoryUI.UpdateItemSlot(item);

                if (amount <= 0)
                    return true;
            }
        }

        while (amount > 0)
        {
            int diff = Mathf.Min(amount, itemData.maxStackAmount);
            if (diff <= 0)
                break;

            ItemInstance newItem = new ItemInstance(itemData);
            newItem.amount = diff;
            amount -= diff;

            if (items.Count >= MAX_INVENTORY_SIZE)
            {
                _floatingTextManager.CreateFloatingText("Full Inventory!!!", Vector3.zero);
                return false;
            }

            items.Add(newItem);
            _inventoryUI.AddItemSlot(newItem);

            if (amount <= 0)
                return true;
        }

        return false;
    }

    public void RemoveItem(ItemInstance itemInstance)
    {
        if (items.Contains(itemInstance))
            items.Remove(itemInstance);
    }

    public bool InventoryIsFull()
    {
        CleanUpEmptyItems();
        return items.Count >= MAX_INVENTORY_SIZE;
    }
}