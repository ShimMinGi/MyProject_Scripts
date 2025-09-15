using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    string GetInteractMsg();
    void OnInteract(Player player);
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData item;
    public int amount = 1;

    public string GetInteractMsg()
    {
        if (item == null)
            return "Pickup Unknown";
        else
            return string.Format("Pickup {0} {1}", item.displayName, amount);
    }

    public void OnInteract(Player player)
    {
        if (!player.AddItem(item, amount))
        {
            return;
        }
        Destroy(gameObject);
    }
}