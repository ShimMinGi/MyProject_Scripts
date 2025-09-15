using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum ItemType
{
    Equipable,
    Consumable,
}

public enum ConsumableType
{
    Health,
}

[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public int value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public ItemType type;
    public GameObject dropPrefab;

    [Header("Icon")]
    public Sprite icon;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Equip")]
    public GameObject equipPrefab;
    public EquipSlotType equipSlot;

    [Header("State")]
    public bool isEquipped;
}
