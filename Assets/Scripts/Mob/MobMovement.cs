using UnityEngine;

public class MobMovement : MonoBehaviour
{
    public float speed;
    private Vector2 motionVector;
    private Rigidbody2D rigidBody;
    private Animator animator;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // set vector towards the provided position from the mob's position
    public void SetMotionVector(Vector3 pos)
    {
        motionVector = (pos - transform.position).normalized;
        animator.SetBool("isRunning", true);
    }

    public void SetMotionless()
    {
        motionVector = Vector2.zero;
        animator.SetBool("isRunning", false);
    }

    // set direction mob is facing
    public void SetDirection()
    {
        if (motionVector.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = motionVector.x > 0 ? -scale.y : scale.y;
            transform.localScale = scale;
        }
    }

    // set velocity
    public void Move()
    {
        SetDirection();
        rigidBody.velocity = motionVector * speed;
    }

}
