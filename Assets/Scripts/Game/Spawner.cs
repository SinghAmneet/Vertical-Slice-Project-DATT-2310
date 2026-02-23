using UnityEngine;

public class Spawner : MonoBehaviour
{
    public ItemData[] itemDataList; // will spawn random items in the list
    public int amount = 1; // amount of items to spawn
    public float randomSpawnRadius; // radius to randomly spawn items in

    void Start()
    {
        ItemSpawner itemSpawner = GetComponent<ItemSpawner>();
        for (int i = 0; i < amount; i ++)
        {
            Vector2 randPos = (Vector2) transform.position + Random.insideUnitCircle * randomSpawnRadius;
            itemSpawner.SpawnRandom(itemDataList, transform.parent.transform, randPos);
        }
        
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, randomSpawnRadius);
    }
}
