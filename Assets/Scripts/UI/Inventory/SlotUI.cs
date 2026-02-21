using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour, 
    IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler,
    IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private int index;

    private Image icon;
    private Image slot;
    private Outline outline;

    private Inventory inventory; // the inventory system
    private Canvas dragCanvas; // canvas for dragging slot icons

    public Color SelectColor = new Color(0.5f, 0.5f, 0.5f);
    public Color DeselectColor = new Color(1f, 1f, 1f);

    private bool hovering = false;

    public void Setup(int i)
    {
        index = i;

        transform.localScale = Vector3.one; // for some reason the scale increases randomly so set to 1 in case it happens

        outline = GetComponent<Outline>();

        icon = transform.GetChild(0).GetComponent<Image>(); // get image component icon under the slot
        slot = GetComponent<Image>(); // get image component of the slot
        RemoveItem(); // disable icon and set deselect
        Highlight(false);

        name = i.ToString();
    }

    public void SetInventory(Inventory inv)
    {
        inventory = inv;
    }

    public void SetDragCanvas(Canvas canvas)
    {
        dragCanvas = canvas;
    }

    public bool IsHovering()
    {
        return hovering;
    }

    // started dragging slot
    public void OnBeginDrag(PointerEventData data)
    {
        // put slot icon into the drag canvas
        icon.transform.SetParent(dragCanvas.transform, true);
    }

    // when slot panel gets clicked, select it
    public void OnPointerClick(PointerEventData data)
    {
        inventory.Select(index);
    }

    // hovering over slot
    public void OnPointerEnter(PointerEventData data)
    {
        hovering = true;
        Highlight(true);
    }

    // stopped hovering over slot
    public void OnPointerExit(PointerEventData data)
    {
        hovering = false;
        Highlight(false);
    }

    // move icon to mouse position
    public void OnDrag(PointerEventData data)
    {
        if (icon.enabled)
        {
            icon.transform.position = data.position;
        }
    }

    // reset icon to original position
    public void OnEndDrag(PointerEventData data)
    {
        icon.transform.SetParent(slot.transform, false);
        icon.transform.localPosition = Vector3.zero;
        
        if (!hovering)
        {
            inventory.invUI.CheckOnDragDrop(index);
        }
    }

    // highlight or unhighlight slot
    public void Highlight(bool show)
    {
        slot.color = show ? SelectColor : DeselectColor;
    }

    // show or hide outline
    public void UpdateSelect(bool show)
    {
        outline.enabled = show;
    }

    // display sprite
    public void AddItem(Item item)
    {
        icon.enabled = true;
        icon.sprite = item.data.sprite;
    }

    // remove sprite
    public void RemoveItem()
    {
        UpdateSelect(false);
        icon.enabled = false;
        icon.sprite = null;
    }

    
}
