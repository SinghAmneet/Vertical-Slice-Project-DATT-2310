using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(SortingGroup))]
public class YSort : MonoBehaviour
{
    private SortingGroup sg;

    void Awake()
    {
        sg = GetComponent<SortingGroup>();
    }

    void LateUpdate()
    {
        sg.sortingOrder = -(int)(transform.position.y * 100);
    }
}