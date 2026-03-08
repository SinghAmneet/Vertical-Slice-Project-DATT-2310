using UnityEngine;

public class JudgementPopup : MonoBehaviour
{
    [Header("Timing")]
    public float lifetime = 0.6f;

    [Header("Scale Pop")]
    public float startScaleMultiplier = 0.8f;   // starts slightly smaller
    public float popScaleMultiplier = 1.1f;     // grows a little bigger than normal
    public float popDuration = 0.12f;           // how fast the pop happens

    private SpriteRenderer sr;
    private float timer;
    private Vector3 baseScale;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        baseScale = transform.localScale;

        // Start slightly smaller for a subtle pop-in effect
        transform.localScale = baseScale * startScaleMultiplier;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // --- Scale pop animation ---
        if (timer <= popDuration)
        {
            float t = timer / popDuration;

            // First half: grow from small -> slightly larger than normal
            // Second half: settle back to normal size
            if (t < 0.5f)
            {
                float growT = t / 0.5f;
                transform.localScale = Vector3.Lerp(
                    baseScale * startScaleMultiplier,
                    baseScale * popScaleMultiplier,
                    growT
                );
            }
            else
            {
                float settleT = (t - 0.5f) / 0.5f;
                transform.localScale = Vector3.Lerp(
                    baseScale * popScaleMultiplier,
                    baseScale,
                    settleT
                );
            }
        }
        else
        {
            transform.localScale = baseScale;
        }

        // --- Fade out in place ---
        if (sr != null)
        {
            Color c = sr.color;
            c.a = Mathf.Lerp(1f, 0f, timer / lifetime);
            sr.color = c;
        }

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}