using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDummyMobDeath : MonoBehaviour
{
    [Header("Drop")]
    [SerializeField] private ItemSpawner itemSpawner;
    [SerializeField] private ItemData mushroomDrop;
    [SerializeField] private Transform dropPoint;

    [Header("Optional")]
    [SerializeField] private Animator animator;
    [SerializeField] private string deathTrigger = "Die";
    [SerializeField] private bool destroyAfterDrop = true;

    private Health health;
    private bool isDead;

    private void Awake()
    {
        health = GetComponent<Health>();

        if (health != null)
        {
            health.OnDeath += HandleDeath;
        }
    }

    private void HandleDeath()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null && !string.IsNullOrEmpty(deathTrigger))
        {
            animator.SetTrigger(deathTrigger);
        }

        DropItem();

        if (destroyAfterDrop)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void DropItem()
    {
        if (itemSpawner == null || mushroomDrop == null) return;

        Vector3 spawnPos = dropPoint != null ? dropPoint.position : transform.position;
        itemSpawner.Spawn(mushroomDrop, null, spawnPos);
    }
}
