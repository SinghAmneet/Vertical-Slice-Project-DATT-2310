using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingCredits : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform creditsContent;
    [SerializeField] private StartMenu startMenu;

    [Header("Scroll Settings")]
    [SerializeField] private float scrollSpeed = 40f;
    [SerializeField] private float startY = -800f;
    [SerializeField] private float endY = 1200f;

    [Header("Options")]
    [SerializeField] private bool playOnEnable = true;
    [SerializeField] private bool loop = true;

    [Header("Exit Settings")]
    [SerializeField] private float inputDelay = 0.5f; // prevents instant skip

    private bool isScrolling;
    private float timer;

    private void OnEnable()
    {
        ResetCredits();
        timer = 0f;

        if (playOnEnable)
        {
            StartScrolling();
        }
    }

    private void Update()
    {
        timer += Time.unscaledDeltaTime;

        HandleScrolling();
        HandleExitInput();
    }

    private void HandleScrolling()
    {
        if (!isScrolling || creditsContent == null) return;

        Vector2 pos = creditsContent.anchoredPosition;
        pos.y += scrollSpeed * Time.unscaledDeltaTime;
        creditsContent.anchoredPosition = pos;

        if (pos.y >= endY)
        {
            if (loop)
            {
                ResetCredits();
            }
            else
            {
                isScrolling = false;
            }
        }
    }

    private void HandleExitInput()
    {
        // Prevent accidental instant exit
        if (timer < inputDelay) return;

        // Click OR press Escape
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (startMenu != null)
            {
                startMenu.toggleCredits();
            }
        }
    }

    public void StartScrolling()
    {
        isScrolling = true;
    }

    public void StopScrolling()
    {
        isScrolling = false;
    }

    public void ResetCredits()
    {
        if (creditsContent == null) return;

        Vector2 pos = creditsContent.anchoredPosition;
        pos.y = startY;
        creditsContent.anchoredPosition = pos;
    }
}
