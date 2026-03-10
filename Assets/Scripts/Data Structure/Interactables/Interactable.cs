using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public GameObject interactUIPrefab;
    private GameObject interactUI;

    private void Start()
    {
        interactUI = Instantiate(interactUIPrefab, gameObject.transform);
        UpdateIndicator(false);
    }

    // show that the object can be interacted with
    public virtual void UpdateIndicator(bool show)
    {
        interactUI.SetActive(show);
        //if (show) Debug.Log("near " + gameObject.name);
    }

    // when object gets interacted with
    public virtual void Use(GameObject plr) { }
}
