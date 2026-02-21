using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
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

        // the only child of Canvas is a TMP, and set its text to the object's name
        textCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.name;

        spriteRender.sprite = data.sprite; // set object sprite to sprite provided in data
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
