using Unity.VisualScripting;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public GameObject mobPrefab;

    public MobData mobData;

    private void Start()
    {
        if (mobData != null) {
            Spawn(mobData, transform.parent.transform, transform.position);
            Destroy(gameObject);
        }
    }

    public void Spawn(MobData data, Transform parent, Vector2 pos)
    {
        GameObject prefab = data.mobPrefab != null ? data.mobPrefab : mobPrefab;
        GameObject obj = Instantiate(prefab, parent, true);
        obj.transform.position = pos;
        obj.name = data.name;

        obj.GetComponent<Mob>().SetData(data);
    }
}
