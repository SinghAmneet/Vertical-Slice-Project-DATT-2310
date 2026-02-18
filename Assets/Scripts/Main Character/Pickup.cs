using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private List<GameObject> objInRange = new();
    private Inventory inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Take();
        }
    }

    // put closest object in range into the player's inventory
    public void Take()
    {
        if (objInRange.Count > 0)
        {
            GameObject obj = GetClosestObj();
            bool success = inventory.Add(obj.GetComponent<Item>());

            // if item was added to inventory
            if (success)
            {
                objInRange.Remove(obj);
                obj.SetActive(false);
            }
        }
    }

    private GameObject GetClosestObj()
    {
        // if there's only 1 obj in range, return it
        if (objInRange.Count == 1)
        {
            return objInRange[0];
        }

        GameObject closestObj = objInRange[0];
        float closestDist = Vector2.Distance(transform.position, closestObj.transform.position);

        // get object that is closest to the player
        for (int i = 1; i < objInRange.Count; i ++)
        {
            GameObject obj = objInRange[i];
            float dist = Vector2.Distance(transform.position, obj.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestObj = obj;
            }
        }
        return closestObj;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (!objInRange.Contains(obj) && collision.CompareTag("Item"))
        {
            objInRange.Add(obj);
            Debug.Log("collided with " + obj.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (objInRange.Contains(obj) && collision.CompareTag("Item"))
        {
            objInRange.Remove(obj);
            Debug.Log("stopped colliding with " + obj.name);
        }
    }
}
