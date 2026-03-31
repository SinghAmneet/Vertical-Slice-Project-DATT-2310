using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCameraFollowClamp : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Clamp")]
    [SerializeField] private float minX = 0f;
    [SerializeField] private float maxX = 103.4f;

    [Header("Fixed Y")]
    [SerializeField] private float fixedY = 12.4f;

    [SerializeField] private float z = 0f;

    private void LateUpdate()
    {
        if (player == null) return;

        float clampedX = Mathf.Clamp(player.position.x, minX, maxX);

        transform.position = new Vector3(clampedX, fixedY, z);
    }
}
