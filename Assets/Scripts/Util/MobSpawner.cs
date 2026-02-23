using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public GameObject mobPrefab;

    public void Spawn(MobData data, Transform parent, Vector2 pos)
    {
        GameObject obj = Instantiate(mobPrefab, parent);
        obj.transform.position = pos;

        obj.GetComponent<Mob>().SetData(data);
    }
}
