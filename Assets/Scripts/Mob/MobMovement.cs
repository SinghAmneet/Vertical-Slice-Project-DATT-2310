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

    }

    public void SetMotionless()
    {
        motionVector = Vector2.zero;
    }

    // set velocity
    public void Move()
    {
        
    }

}
