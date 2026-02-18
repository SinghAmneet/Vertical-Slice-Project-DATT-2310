using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject inventory;
    private InventoryUI invUI;

    public int slotCount;

    private List<Item> items = new();

    private Item selectedItem;

    private void Awake()
    {
        // get ui component and create new slots
        invUI = inventory.GetComponent<InventoryUI>();
        invUI.Setup(slotCount);
    }

    // select or deselect item
    public void Select(int i)
    {
        // if selected item doesn't currently equal to the new selected item
        if (selectedItem != items[i])
        {
            selectedItem = items[i];
        }
        else // remove selection if selecting the same item
        { 
            selectedItem = null;
        }
    }

    // tries to insert item, and returns if it was successful or not
    public bool Add(Item item)
    {
        if (items.Count < slotCount)
        {
            items.Add(item);
            invUI.AddItemToSlot(item, items.Count - 1);
            return true;
        }

        Debug.Log("Inventory is full!");
        return false;
    }

    // drops the selected item
    public void Drop()
    {
        if (selectedItem != null)
        {

        }
    }

    // remove item using the actual item
    public void Remove(Item item)
    {
        invUI.RemoveItemFromSlot(item, items.IndexOf(item));
        items.Remove(item);
    }

    // remove item using item's index
    public void Remove(int i)
    {
        invUI.RemoveItemFromSlot(items[i], i);
        items.RemoveAt(i);
    }
}
