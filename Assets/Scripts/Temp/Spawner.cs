using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public ItemData itemData;
    public GameObject itemPrefab;

    void Start()
    {
        GameObject obj = Instantiate(itemPrefab, transform);
        Item item = obj.GetComponent<Item>();
        obj.transform.name = itemData.name;

        item.SetData(itemData);
    }
}
