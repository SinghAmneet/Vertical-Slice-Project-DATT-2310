using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // show that the object can be interacted with
    public virtual void UpdateIndicator(bool show)
    {
        if (show) Debug.Log("near " + gameObject.name);
    }

    // when object gets interacted with
    public virtual void Use(GameObject plr) { }
}
