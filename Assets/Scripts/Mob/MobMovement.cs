using UnityEngine;

public class MobMovement : MonoBehaviour
{
    public float speed;
    private Vector2 motionVector = Vector2.zero;
    private Rigidbody2D rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // set vector towards the provided position from the mob's position
    public void SetMotionVector(Vector3 pos)
    {
        motionVector = (pos - transform.position).normalized;
    }

    public void SetMotionless()
    {
        motionVector = Vector2.zero;
    }

    // set direction mob is facing
    public void SetDirection()
    {

    }

    // set velocity
    public void Move()
    {
        SetDirection();
        rigidBody.velocity = motionVector * speed;
    }

}
