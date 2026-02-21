using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    private float health;
    public HealthUI healthUI;

    public event Action OnDeath; // death event when health reaches 0

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        health = maxHealth;
        if (healthUI != null) healthUI.Setup(Mathf.CeilToInt(maxHealth));
    }

    // health testing
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Z))
    //    {
    //        Deplete(0.25f);
    //    } else if (Input.GetKeyDown(KeyCode.X))
    //    {
    //        Heal(0.5f);
    //    }
    //}

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
        Setup();
    }

    private void UpdateHealth(float newHp)
    {
        health = Mathf.Clamp(newHp, 0, maxHealth);
        if (healthUI != null) healthUI.UpdateHearts(health); // update UI
    }

    // add to hp
    public void Heal(float hp)
    {
        UpdateHealth(health + hp);
    }

    // subtract from hp
    public void Deplete(float hp)
    {
        UpdateHealth(health - hp);

        if (health == 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnDeath?.Invoke(); // call death event
        Debug.Log(gameObject.name + " has died!!!!!");
    }
}
