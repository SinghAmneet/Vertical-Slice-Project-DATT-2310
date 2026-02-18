using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour, IPointerClickHandler
{
    private int index;
    private Image icon;

    public void Setup(int i)
    {
        index = i;
        icon = transform.GetChild(0).gameObject.GetComponent<Image>(); // get image component icon under the slot
        
    }

    // when slot panel gets clicked
    public void OnPointerClick(PointerEventData data)
    {

    }

    public void AddItem(Item item)
    {
        Debug.Log("add to ui");
        icon.sprite = item.data.sprite;
    }

    public void RemoveItem(Item item)
    {
        icon.sprite = null;
    }

    
}
