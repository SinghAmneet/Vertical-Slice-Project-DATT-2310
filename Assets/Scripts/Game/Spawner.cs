using UnityEngine;

public class Spawner : MonoBehaviour
{
    public ItemData[] itemDataList; // will spawn random items in the list
    public int amount = 1; // amount of items to spawn
    public float randomSpawnRadius; // radius to randomly spawn items in

    void Start()
    {
        GetComponent<ItemSpawner>().SpawnRadius(
            itemDataList, 
            transform.parent.transform, 
            transform.position, 
            amount, 
            randomSpawnRadius);
        
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, randomSpawnRadius);
    }
}
