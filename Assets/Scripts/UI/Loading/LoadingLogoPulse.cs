using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingLogoPulse : MonoBehaviour
{
    [Header("Pulse")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float minAlpha = 0.35f;
    [SerializeField] private float maxAlpha = 1f;

    [Header("Optional Scale Pulse")]
    [SerializeField] private bool pulseScale = true;
    [SerializeField] private float minScale = 0.95f;
    [SerializeField] private float maxScale = 1.05f;

    private Image image;
    private Vector3 baseScale;

    private void Awake()
    {
        image = GetComponent<Image>();
        baseScale = transform.localScale;
    }

    private void Update()
    {
        float t = (Mathf.Sin(Time.unscaledTime * speed) + 1f) * 0.5f;

        float alpha = Mathf.Lerp(minAlpha, maxAlpha, t);

        if (image != null)
        {
            Color c = image.color;
            c.a = alpha;
            image.color = c;
        }

        if (pulseScale)
        {
            float scale = Mathf.Lerp(minScale, maxScale, t);
            transform.localScale = baseScale * scale;
        }
    }
}
