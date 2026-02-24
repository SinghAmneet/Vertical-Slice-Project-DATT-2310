using UnityEngine;

public class MobMovement : MonoBehaviour
{
    public float speed;
    private Vector2 motionVector = Vector2.zero;
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
        if (animator != null) animator.SetBool("isRunning", true);
    }

    public void SetMotionless()
    {
        motionVector = Vector2.zero;
        if (animator != null) animator.SetBool("isRunning", false);
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
