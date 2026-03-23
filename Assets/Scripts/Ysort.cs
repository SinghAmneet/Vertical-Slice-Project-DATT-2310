using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(SortingGroup))]
public class YSort : MonoBehaviour
{
    public int sortingOrderBase = 5000;
    public int offset = 0;
    public bool runOnlyOnce = false;

    private SortingGroup sortingGroup;
    private float lastY;

    void Awake()
    {
        sortingGroup = GetComponent<SortingGroup>();
    }

    void LateUpdate()
    {
        if (runOnlyOnce)
        {
            if (Mathf.Approximately(lastY, transform.position.y)) return;
            lastY = transform.position.y;
        }

        sortingGroup.sortingOrder = (int)(sortingOrderBase - transform.position.y * 100) + offset;
    }
}