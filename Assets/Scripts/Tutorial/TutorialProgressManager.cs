using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialProgressManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TutorialCharacterController2D playerController;
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private Health mushroomHealth;

    [Header("Targets")]
    [SerializeField] private int requiredPickupCount = 2;

    [Header("Debug / Read Only")]
    [SerializeField] private bool hasMoved;
    [SerializeField] private bool pickedUpItems;
    [SerializeField] private bool droppedItems;
    [SerializeField] private bool killedMushroom;
    [SerializeField] private bool tutorialComplete;

    private int lastInventoryCount = 0;
    private bool startedDropTracking = false;

    public bool HasMoved => hasMoved;
    public bool PickedUpItems => pickedUpItems;
    public bool DroppedItems => droppedItems;
    public bool KilledMushroom => killedMushroom;
    public bool TutorialComplete => tutorialComplete;

    private void Awake()
    {
        if (mushroomHealth != null)
        {
            mushroomHealth.OnDeath += OnMushroomKilled;
        }
    }

    private void Start()
    {
        if (playerInventory != null)
        {
            lastInventoryCount = playerInventory.GetItems().Count;
        }
    }

    private void Update()
    {
        TrackMovement();
        TrackInventoryProgress();
        UpdateCompletion();
    }

    private void TrackMovement()
    {
        if (playerController != null && playerController.HasMoved)
        {
            hasMoved = true;
        }
    }

    private void TrackInventoryProgress()
    {
        if (playerInventory == null) return;

        int currentCount = playerInventory.GetItems().Count;

        //Picked up enough items
        if (!pickedUpItems && currentCount >= requiredPickupCount)
        {
            pickedUpItems = true;
            startedDropTracking = true;
        }

        // After reaching the pickup goal, count dropping back down to zero
        if (startedDropTracking && pickedUpItems && currentCount == 0)
        {
            droppedItems = true;
        }

        lastInventoryCount = currentCount;
    }

    private void OnMushroomKilled()
    {
        killedMushroom = true;
    }

    private void UpdateCompletion()
    {
        tutorialComplete = hasMoved && pickedUpItems && droppedItems && killedMushroom;
    }
}
