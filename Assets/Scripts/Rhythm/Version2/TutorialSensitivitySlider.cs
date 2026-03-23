using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialSensitivitySlider : MonoBehaviour
{
    [Header("References")]
    public CursorManager cursorManager;
    public Transform knob;
    public TMP_Text valueText;

    [Header("Slider Settings")]
    public float minValue = 0.1f;
    public float maxValue = 3.0f;

    [Tooltip("Half the width of the slider bar. If the bar goes from -4.8 to 4.8, use 4.8.")]
    public float sliderHalfWidth = 4.8f;

    public float clickHeightTolerance = 0.6f;

    private bool dragging = false;

    void Start()
    {
        ApplyValue(1.0f);
    }

    void Update()
    {
        if (cursorManager == null)
            return;

        Vector2 cursorPos = cursorManager.GetWorldPosition();

        if (Input.GetMouseButtonDown(0) && IsCursorOverSlider(cursorPos))
        {
            dragging = true;
            SetFromCursor(cursorPos.x);
        }

        if (dragging && Input.GetMouseButton(0))
        {
            SetFromCursor(cursorPos.x);
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }
    }

    bool IsCursorOverSlider(Vector2 cursorPos)
    {
        Vector3 p = transform.position;

        bool insideX =
            cursorPos.x >= p.x - sliderHalfWidth - 0.3f &&
            cursorPos.x <= p.x + sliderHalfWidth + 0.3f;

        bool insideY = Mathf.Abs(cursorPos.y - p.y) <= clickHeightTolerance;

        return insideX && insideY;
    }

    void SetFromCursor(float cursorX)
    {
        float localX = cursorX - transform.position.x;
        localX = Mathf.Clamp(localX, -sliderHalfWidth, sliderHalfWidth);

        float t = Mathf.InverseLerp(-sliderHalfWidth, sliderHalfWidth, localX);
        float value = Mathf.Lerp(minValue, maxValue, t);

        ApplyValue(value);
    }

    void ApplyValue(float value)
    {
        value = Mathf.Clamp(value, minValue, maxValue);

        if (cursorManager != null)
            cursorManager.SetSensitivity(value);

        if (knob != null)
        {
            float t = Mathf.InverseLerp(minValue, maxValue, value);
            float x = Mathf.Lerp(-sliderHalfWidth, sliderHalfWidth, t);
            knob.localPosition = new Vector3(x, 0f, 0f);
        }

        if (valueText != null)
            valueText.text = "Sensitivity: " + value.ToString("F2") + "x";
    }
}