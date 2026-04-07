using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    private float health;
    public HealthUI healthUI;
    public AudioSource hurtSound;

    public event Action OnDeath; // death event when health reaches 0
    public event Action OnDamage; // when character takes damage

    private bool invulnerable = false;

    private void Awake()
    {
        if (maxHealth != 0) Setup();
    }

    private void Setup()
    {
        health = maxHealth;
        healthUI?.Setup(Mathf.CeilToInt(maxHealth));
    }

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
        Setup();
    }

    public void SetInvulnerable(bool invulnerable)
    {
        this.invulnerable = invulnerable;
    }

    public float GetHealth()
    {
        return health;
    }

    private void UpdateHealth(float newHp)
    {
        if (invulnerable) return;
        health = Mathf.Clamp(newHp, 0, maxHealth);
        healthUI?.UpdateHearts(health); // update UI
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
        if (health == 0) { Die(); } else { hurtSound?.PlayOneShot(hurtSound.clip); OnDamage?.Invoke(); }
    }

    public void Die()
    {
        OnDeath?.Invoke(); // call death event
        //Debug.Log(gameObject.name + " has died!!!!!");
    }
}
