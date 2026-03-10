using System.Collections.Generic;
using UnityEngine;
using TMPro;

// class for items during runtime
public class Item : MonoBehaviour
{
    private SpriteRenderer spriteRender;
    public ItemData data;

    private GameObject textCanvas; // the canvas parenting the object's world text

    private void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();

        // the only child of item is a Canvas
        textCanvas = transform.GetChild(0).gameObject;

        UpdateIndicator(false); // hide name
    }

    // set item data
    public void SetData(ItemData data)
    {
        this.data = data;
        gameObject.name = data.name;

        // the first child of Canvas is the name text object, and set its text to the object's name
        textCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.name;

        SetStats();
        SetTextSize();

        spriteRender.sprite = data.sprite; // set object sprite to sprite provided in data
    }

    // temporary until item sprites have consistent sizes
    public void SetTextSize()
    {
        transform.localScale *= data.scaleMultiplier;
        if (data.scaleMultiplier < 1)
        {
            textCanvas.transform.position += Vector3.up * 2.5f;
            textCanvas.transform.localScale *= 7f;
            GetComponent<CircleCollider2D>().radius = 10;
        }
    }

    // display stats above item if it's a food
    private void SetStats()
    {
        // the second child of Canvas is a template stat text object
        if (data is FoodData foodData)
        {
            GameObject template = textCanvas.transform.GetChild(1).gameObject;
            template.GetComponent<TextMeshProUGUI>().text = foodData.type.ToString();
            template.GetComponent<TextMeshProUGUI>().color = foodData.GetColor();
        }
        
    }

    // show or hide object name
    public void UpdateIndicator(bool show)
    {
        //enable or disable text canvas, which is a parent of the textObj
        textCanvas.SetActive(show);
    }

    public void Use(GameObject plr)
    {
        data.Use(plr);
    }
}
