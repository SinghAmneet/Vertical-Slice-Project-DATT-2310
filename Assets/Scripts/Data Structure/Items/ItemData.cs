using UnityEngine;

// An abstract class for items
public abstract class ItemData : ScriptableObject
{
    public Sprite sprite;
    public float scaleMultiplier = 1f;
    public abstract void Use(GameObject plr);
}