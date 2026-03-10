using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

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

    private GameObject statPanel;

    private bool hovering = false;

    public void Setup(int i)
    {
        index = i;

        transform.localScale = Vector3.one; // for some reason the scale increases randomly so set to 1 in case it happens

        outline = GetComponent<Outline>();

        icon = transform.GetChild(0).GetComponent<Image>(); // get image component icon under the slot

        statPanel = transform.GetChild(1).gameObject;
        statPanel.transform.GetChild(0).gameObject.SetActive(false);
        statPanel.SetActive(false);

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
        statPanel.SetActive(false);
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
        statPanel.SetActive(true);
        hovering = true;
        Highlight(true);
    }

    // stopped hovering over slot
    public void OnPointerExit(PointerEventData data)
    {
        statPanel.SetActive(false);
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
        } else
        {
            statPanel.SetActive(true);
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

    public TextMeshProUGUI CreateStatObj(GameObject template, string str)
    {
        GameObject statObj = Instantiate(template, statPanel.transform);
        statObj.GetComponent<TextMeshProUGUI>().text = str;
        statObj.SetActive(true);
        return statObj.GetComponent<TextMeshProUGUI>();
    }

    public void FillStats(Item item)
    {
        if (item.data is FoodData foodData)
        {
            GameObject template = statPanel.transform.GetChild(0).gameObject;
            //template.SetActive(false);
            TextMeshProUGUI itemName = CreateStatObj(template, foodData.name);
            itemName.fontStyle = FontStyles.Bold;  
            TextMeshProUGUI type = CreateStatObj(template, foodData.type.ToString());
            type.color = foodData.GetColor();
            type.fontStyle = FontStyles.Italic;
            for (int i = 0; i < foodData.stats.Count; i++)
            {
                Stat stat = foodData.stats[i];
                TextMeshProUGUI statObj = CreateStatObj(template, $"{stat.stat.ToString()}: {stat.value}");
                statObj.color = Color.black;
            }
            
        }
    }

    public void ClearStats()
    {
        for (int i = statPanel.transform.childCount - 1; i > 0; i --)
        {
            GameObject statObj = statPanel.transform.GetChild(i).gameObject;
            Destroy(statObj);
        }
    }

    // display sprite
    public void AddItem(Item item)
    {
        icon.enabled = true;
        icon.sprite = item.data.sprite;
        ClearStats();
        FillStats(item);
    }

    // remove sprite
    public void RemoveItem()
    {
        UpdateSelect(false);
        icon.enabled = false;
        icon.sprite = null;
        ClearStats();
    }

    
}
