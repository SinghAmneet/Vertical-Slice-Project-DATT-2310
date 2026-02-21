using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject inventory; // the in game inventory UI
   [HideInInspector] public InventoryUI invUI; // inventory UI component

    public int slotCount;
    public Transform dropPoint;

    private List<Item> items;
    private int selectedIndex = -1;

    private Item GetSelectedItem()
    {
        if (selectedIndex < 0) return null;
        return items[selectedIndex];
    }

    private void Awake()
    {
        // get UI component and create new UI slots
        invUI = inventory.GetComponent<InventoryUI>();
        invUI.Setup(slotCount, GetComponent<Inventory>());

        // define size of items list using slotCount
        items = new(new Item[slotCount]); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Drop(selectedIndex);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Use();
        }

        // get number inputs from 1 to number of slots
        for (int i = 0; i < slotCount; i ++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                Select(i);
            }
        }
    }

    private void Use()
    {
        if (selectedIndex >= 0)
        {
            GetSelectedItem().Use(gameObject);
        }
    }

    // select or deselect item
    public void Select(int i)
    {
        // return if index is above item count
        if (i > items.Count) return;
        
        if (selectedIndex == i)
        {
            invUI.DeselectItem(i);
            selectedIndex = -1;
        } else
        {
            if (selectedIndex >= 0)
            {
                invUI.DeselectItem(selectedIndex);
            }
            invUI.SelectItem(i);
            selectedIndex = i;
        }
    }

    // swap the values of two slot indexes
    public void Swap(int i0,  int i1)
    {
        if (i0 == i1) return; // return if they're the same index

        // swap values
        Item i1Value = items[i1];
        items[i1] = items[i0];
        items[i0] = i1Value;
        invUI.AddItemToSlot(items[i0], i0);
        invUI.AddItemToSlot(items[i1], i1);

        if (selectedIndex < 0) return; // return if no slot is selected

        // check if one of the swapped items were selected
        if (selectedIndex == i0)
        {
            Select(i1);
        } else if (selectedIndex == i1)
        {
            Select(i0);
        }
    }

    // gets the first available slot index without an item, returns -1 if no available slot
    private int getAvailableIndex()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null) return i;
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
            items[i] = item;
            invUI.AddItemToSlot(item, i);
            
            return true;
        }

        Debug.Log("Inventory is full!");
        return false;
    }

    // drops the the item from the provided index
    public void Drop(int i)
    {
        if (i < 0) return;
        Item item = items[i];
        item.transform.position = dropPoint.position;
        item.gameObject.SetActive(true);

        Remove(item);
    }

    // remove item using the actual item
    public void Remove(Item item)
    {
        Remove(items.IndexOf(item));
    }

    // remove item using item's index
    public void Remove(int i)
    {
        if (selectedIndex == i) selectedIndex = -1; 
        invUI.RemoveItemFromSlot(i);
        items[i] = null;
    }
}
