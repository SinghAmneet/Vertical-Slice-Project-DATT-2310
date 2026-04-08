using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePopup : MonoBehaviour
{
    [Header("Timing")]
    public float lifetime;

    [Header("Scale")]
    public float startScaleMultiplier;
    public float endScaleMultiplier;

    [Header("Movement")]
    public float moveDistance;   // how far the fire drifts away
    public bool randomDirection = true;

    [Header("Fade")]
    public bool fadeOut = true;

    private SpriteRenderer sr;
    private float timer;

    private Vector3 baseScale;
    private Vector3 startPos;
    private Vector3 moveDirection;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        baseScale = transform.localScale;
        startPos = transform.position;

        transform.localScale = baseScale * startScaleMultiplier;

        // Pick a random direction
        if (randomDirection)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            moveDirection = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
        }
        else
        {
            moveDirection = Vector3.up;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / lifetime);

        // Expand slowly
        transform.localScale = Vector3.Lerp(
            baseScale * startScaleMultiplier,
            baseScale * endScaleMultiplier,
            t
        );

        // Drift outward from the note
        transform.position = startPos + moveDirection * moveDistance * t;

        // Fade out
        if (fadeOut && sr != null)
        {
            Color c = sr.color;
            c.a = Mathf.Lerp(1f, 0f, t);
            sr.color = c;
        }

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}