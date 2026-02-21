using System.Collections.Generic;
using UnityEngine;

/*
 * Note: the trigger events uses the player's CircleCollider2D.
 * if you want to change the range, change the collider's radius
 */

public class Pickup : MonoBehaviour
{
    private List<GameObject> objsInRange = new();
    private GameObject objInRange;
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

        if (objsInRange.Count > 0)
        {
            SetClosestObj();
        }
    }

    // cleaner way to get the Item component
    private Item GetItem(GameObject obj)
    {
        return obj.GetComponent<Item>();
    }

    // put closest object in range into the player's inventory
    public void Take()
    {
        // if there is an object in range
        if (objInRange != null && IsItem(objInRange))
        {
            bool success = inventory.Add(GetItem(objInRange));

            // if item was added to inventory
            if (success)
            {
                UpdateIndicator(objInRange, false);
                objsInRange.Remove(objInRange);
                objInRange.SetActive(false);
                objInRange = null;
            }
        }
    }

    private GameObject GetClosestObj()
    {
        // if there's only 1 obj in range, return it
        if (objsInRange.Count == 1)
        {
            return objsInRange[0];
        }

        GameObject closestObj = objsInRange[0];
        float closestDist = Vector2.Distance(transform.position, closestObj.transform.position);

        // get object that is closest to the player
        for (int i = 1; i < objsInRange.Count; i ++)
        {
            GameObject obj = objsInRange[i];

            // get distance from player to object
            float dist = Vector2.Distance(transform.position, obj.transform.position);

            if (dist < closestDist)
            {
                closestDist = dist;
                closestObj = obj;
            }
        }
        return closestObj;
    }

    // show or hide object name
    private void UpdateIndicator(GameObject obj, bool show)
    {
        if (IsItem(obj))
        {
            GetItem(obj).UpdateIndicator(show);
        } else
        {

        }
        
    }

    // set closest obj and display an indicator 
    private void SetClosestObj()
    {
        GameObject obj = GetClosestObj();
        if (obj != objInRange && objInRange != null)
        {
            UpdateIndicator(objInRange, false);
        } else if (obj == objInRange)
        {
            return; // return if closest object is the same
        }

        UpdateIndicator(obj, true);
        objInRange = obj;
    }

    private bool IsItem(GameObject obj)
    {
        return obj.CompareTag("Item");
    }

    private bool HasValidTag(Collider2D collision)
    {
        return collision.CompareTag("Item") || collision.CompareTag("Interactable");
    }

    // item enters pickup range
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (!objsInRange.Contains(obj) && HasValidTag(collision))
        {
            objsInRange.Add(obj);
            //Debug.Log("collided with " + obj.name);
        }
        else if (collision.CompareTag("Interactable"))
        {
            
        }
    }

    // item leaves pickup range
    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (objsInRange.Contains(obj) && HasValidTag(collision))
        {
            objsInRange.Remove(obj);

            // if there are no more objects in range 
            if (objsInRange.Count == 0 && objInRange != null)
            {
                UpdateIndicator(objInRange, false);
                objInRange = null;
            }

            //Debug.Log("stopped colliding with " + obj.name);
        }
    }
}
