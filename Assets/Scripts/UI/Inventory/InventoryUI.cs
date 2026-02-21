using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// sets up inventory UI
public class InventoryUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject slotPrefab; // prefab of the slot panel
    private List<SlotUI> slots = new(); // list of created slot panels

    private Inventory inventory; // the inventory system
    public Canvas dragCanvas; // canvas for dragging slot icons

    private int slotCount;
    private bool hovering = false; // if hovering over any slot UI

    public void OnPointerEnter(PointerEventData data)
    {
        hovering = true;
    }

    public void OnPointerExit(PointerEventData data) 
    {
        hovering = false;
    }

    public void Setup(int slotCount, Inventory inv)
    {
        inventory = inv;
        this.slotCount = slotCount;
        // create the provided amount of slot
        for (int i = 0; i < slotCount; i++)
        {
            // create a new slot panel and set it's parent
            GameObject slotObj = Instantiate(slotPrefab);
            slotObj.transform.SetParent(transform);

            // get slot component and set it up
            SlotUI slot = slotObj.GetComponent<SlotUI>();
            slot.Setup(i);
            slot.SetInventory(inv);
            slot.SetDragCanvas(dragCanvas);

            slots.Add(slot); // add slot component to slots list
        }
    }

    public void CheckOnDragDrop(int i)
    {
        // drop item if player is not hovering over a slot
        if (!hovering)
        {
            inventory.Drop(i);
            return;
        } 

        // check which slot player is hovering over
        for (int j = 0; j < slotCount; j++)
        {
            SlotUI slot = slots[j];
            if (slot.IsHovering())
            {
                inventory.Swap(i, j);
                return;
            }
        }
    }

    public void SelectItem(int i)
    {
        slots[i].UpdateSelect(true);
    }

    public void DeselectItem(int i)
    {
        slots[i].UpdateSelect(false);
    }

    public void DeselectAll()
    {
        foreach (SlotUI slot in slots)
        {
            slot.UpdateSelect(false);
        }
    }

    // show item sprite on slot
    public void AddItemToSlot(Item item, int i)
    {
        if (item == null)
        {
            RemoveItemFromSlot(i);
        } else
        {
            slots[i].AddItem(item);
        }
    }

    // remove item sprite from slot
    public void RemoveItemFromSlot(int i)
    {
        slots[i].RemoveItem();
    }
}
