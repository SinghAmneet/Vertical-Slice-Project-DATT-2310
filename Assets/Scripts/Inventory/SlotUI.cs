using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour, IPointerClickHandler
{
    private int index;
    private Image icon;
    private Image slot;
    private Inventory inventory; // the inventory system

    private Color SELECT_COLOR = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    private Color DESELECT_COLOR = new Color(1f, 1f, 1f, 0.5f);

    public void Setup(int i, Inventory inv)
    {
        index = i;

        transform.localScale = Vector3.one; // for some reason the scale increases randomly so set to 1 in case it happens

        icon = transform.GetChild(0).GetComponent<Image>(); // get image component icon under the slot

        slot = GetComponent<Image>();
        RemoveItem(); // disable icon and set deselect color

        name = i.ToString();
        inventory = inv;
    }

    // when slot panel gets clicked
    public void OnPointerClick(PointerEventData data)
    {
        inventory.Select(index);
    }

    public void SelectItem()
    {
        slot.color = SELECT_COLOR;
    }
    
    public void DeselectItem()
    {
        slot.color = DESELECT_COLOR;
    }

    public void AddItem(Item item)
    {
        icon.enabled = true;
        icon.sprite = item.data.sprite;
        
    }

    public void RemoveItem()
    {
        DeselectItem();
        icon.enabled = false;
        icon.sprite = null;
    }

    
}
