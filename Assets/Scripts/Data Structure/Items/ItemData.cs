using UnityEngine;

// An abstract class for items
public abstract class ItemData : ScriptableObject
{
    public Sprite sprite;
    public abstract void Use(GameObject plr);
}