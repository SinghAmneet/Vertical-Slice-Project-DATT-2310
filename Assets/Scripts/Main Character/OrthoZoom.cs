using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class OrthoZoom : MonoBehaviour
{
    [Header("References")]
    public CinemachineVirtualCamera virtualCamera;

    [Header("Zoom Settings")]
    public float zoomSpeed;
    public float minOrthoSize;
    public float maxOrthoSize;
    public bool invertScroll = false;

    [Header("Smoothing")]
    public bool smoothZoom = true;
    public float zoomSmoothSpeed = 10f;

    private float targetOrthoSize;

    void Awake()
    {
        if (virtualCamera == null)
            virtualCamera = GetComponent<CinemachineVirtualCamera>();

        if (virtualCamera == null)
        {
            Debug.LogError("OrthoZoom: No CinemachineVirtualCamera found.");
            enabled = false;
            return;
        }

        targetOrthoSize = virtualCamera.m_Lens.OrthographicSize;
    }

    void Update()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (Mathf.Abs(scroll) > 0.01f)
        {
            float direction = invertScroll ? 1f : -1f;
            targetOrthoSize += scroll * zoomSpeed * direction;
            targetOrthoSize = Mathf.Clamp(targetOrthoSize, minOrthoSize, maxOrthoSize);
        }

        if (smoothZoom)
        {
            float currentSize = virtualCamera.m_Lens.OrthographicSize;
            float newSize = Mathf.Lerp(currentSize, targetOrthoSize, Time.deltaTime * zoomSmoothSpeed);
            virtualCamera.m_Lens.OrthographicSize = newSize;
        }
        else
        {
            virtualCamera.m_Lens.OrthographicSize = targetOrthoSize;
        }
    }
}
