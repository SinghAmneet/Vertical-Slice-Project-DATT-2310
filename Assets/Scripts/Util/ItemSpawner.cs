using UnityEngine;

public class ItemSpawner : MonoBehaviour 
{
    public GameObject itemPrefab;
    public GameObject Spawn(ItemData data, Transform parent, Vector3 pos)
    {
        GameObject obj = Instantiate(itemPrefab, parent);

        Item item = obj.GetComponent<Item>();

        obj.transform.position = pos;
        obj.transform.name = data.name;

        item.SetData(data);
        return obj;
    }

    public GameObject SpawnRandom(ItemData[] dataList, Transform parent, Vector3 pos)
    {
        return Spawn(dataList[Random.Range(0, dataList.Length)], parent, pos);
    }
}
