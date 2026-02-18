using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// sets up inventory UI
public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab; // prefab of the slot panel
    public List<SlotUI> slots = new(); // list of created slot panels

    public void Setup(int slotCount)
    {
        // create the provided amount of slot
        for (int i = 0; i < slotCount; i++)
        {
            // create a new slot panel and set it's parent
            GameObject slotObj = Instantiate(slotPrefab);
            slotObj.transform.SetParent(transform);

            // get slot component and set it up
            SlotUI slot = slotObj.GetComponent<SlotUI>();
            slot.Setup(i);

            slots.Add(slot); // add slot component to slots list
        }
    }

    public void AddItemToSlot(Item item, int i)
    {
        slots[i].AddItem(item);
    }

    public void RemoveItemFromSlot(Item item, int i)
    {
        slots[i].RemoveItem(item);
    }
}
