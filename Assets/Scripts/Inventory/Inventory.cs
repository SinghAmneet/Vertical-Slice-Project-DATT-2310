using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject inventory; // the in game inventory UI
    private InventoryUI invUI; // inventory UI component

    public int slotCount;
    public Transform dropPoint;

    private List<Item> items;
    private Item selectedItem;

    private void Awake()
    {
        // get UI component and create new UI slots
        invUI = inventory.GetComponent<InventoryUI>();
        invUI.Setup(slotCount, GetComponent<Inventory>());

        items = new(new Item[slotCount]);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            Drop();
        }
    }

    // select or deselect item
    public void Select(int i)
    {
        if (i > items.Count || items[i] == null) { return; } // return if there are no stored items

        // if selected item doesn't currently equal to the new selected item
        if (selectedItem != items[i])
        {
            if (selectedItem != null)
            {
                invUI.DeselectItem(items.IndexOf(selectedItem)); // deselect currently selected slot
            }
            invUI.SelectItem(i);
            selectedItem = items[i];
        }
        else // remove selection if selecting the same item
        {
            invUI.DeselectItem(i);
            selectedItem = null;
        }
    }

    // gets the first available slot index without an item, returns -1 if no available slot
    private int getAvailableIndex()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null) { return i; }
        }
        return -1;
    }

    // tries to insert item, and returns if it was successful or not
    public bool Add(Item item)
    {
        int i = getAvailableIndex();

        // inventory is not full
        if (i >= 0) 
        {
            //items.Add(item);
            
            items[i] = item;
            invUI.AddItemToSlot(item, i);
            
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
            // put item object on the drop point
            selectedItem.transform.position = dropPoint.position;
            selectedItem.gameObject.SetActive(true);

            Remove(selectedItem);
            selectedItem = null;
        }
    }

    // remove item using the actual item
    public void Remove(Item item)
    {
        Remove(items.IndexOf(item));
    }

    // remove item using item's index
    public void Remove(int i)
    {
        invUI.RemoveItemFromSlot(i);
        items[i] = null;
    }
}
