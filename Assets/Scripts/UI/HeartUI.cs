using UnityEngine;

public class HeartUI : MonoBehaviour
{
    public Transform barTransform;

    private void Awake()
    {
        barTransform.localScale = Vector3.one;
    }

    public void UpdateProgress(float progress)
    {
        if (barTransform.localScale.x == progress) return;
        barTransform.localScale = new Vector2(progress, 1);
    }

    // restore heart
    public void FullHeal()
    {
        UpdateProgress(1);
    }

    // deplete heart
    public void FullDeplete()
    {
        UpdateProgress(0);
    }
}
