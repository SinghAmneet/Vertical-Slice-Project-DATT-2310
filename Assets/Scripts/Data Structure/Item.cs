using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class for items during runtime
public class Item : MonoBehaviour
{
    private SpriteRenderer spriteRender;
    public ItemData data;
    public string itemName;

    private void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }

    // set item data
    public void SetData(ItemData data)
    {
        this.data = data;
        spriteRender.sprite = data.sprite; // set object sprite to sprite provided in data
    }

    public void Use(GameObject plr)
    {
        data.Use(plr);
    }
}
