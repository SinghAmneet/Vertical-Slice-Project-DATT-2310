using UnityEngine;

public class ItemSpawner : MonoBehaviour 
{
    public GameObject itemPrefab;

    // spawn and set up item object
    public GameObject Spawn(ItemData data, Transform parent, Vector3 pos)
    {
        GameObject obj = Instantiate(itemPrefab, parent);

        Item item = obj.GetComponent<Item>();

        obj.transform.position = pos;
        obj.transform.name = data.name;

        item.SetData(data);
        return obj;
    }

    // get random item from item list
    public GameObject SpawnRandom(ItemData[] dataList, Transform parent, Vector3 pos)
    {
        return Spawn(dataList[Random.Range(0, dataList.Length)], parent, pos);
    }

    // spawn in a random point in the radius
    public void SpawnRandom(ItemData[] dataList, Transform parent, Vector2 pos, int dropAmount, float radius)
    {
        for (int i = 0; i < dropAmount; i++)
        {
            Vector2 randPos = pos + Random.insideUnitCircle * radius;
            SpawnRandom(dataList, parent, randPos);
        }
    }
}
