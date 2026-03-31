using UnityEngine;

public class PotIndicator : MonoBehaviour
{
    public float radius = 10f;
    public float distToHide = 40f;
    public Transform plr;
    public Transform pot;

    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private bool InHideRange()
    {
        return Vector2.Distance(pot.position, plr.position) < distToHide;
    }

    private Vector2 GetDirection()
    {
        return (pot.position - plr.position).normalized;
    }

    private Vector2 GetPos(Vector2 dir)
    {
        return (Vector2) plr.position + dir * radius;
    }

    private float GetAngle(Vector2 dir)
    {
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    void Update()
    {
        if (InHideRange())
        {
            sprite.enabled = false;
        } else
        {
            sprite.enabled = true;
            Vector2 dir = GetDirection();
            Vector2 pos = GetPos(dir);
            float angle = GetAngle(dir);

            transform.position = pos;
            transform.rotation = Quaternion.Euler(0, 0, angle - 180);
        }
    }
}
