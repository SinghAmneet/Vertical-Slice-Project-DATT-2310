using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    [RequireComponent(typeof(Rigidbody2D))]
    Ensures this GameObject always has a Rigidbody2D.
    Unity will automatically add one if missing.
*/
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{

    Rigidbody2D rigidbody2d;
    [SerializeField] float speed = 2f; // Movement speed in units per second
    [SerializeField] Transform graphics;
    Vector2 motionVector;
    Animator animator;
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update(){

        /*
            Get raw AWSD/arrow key input and normalize so diagonal
            movement is not faster than horizontal/vertical movement.
        */
        motionVector = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        // Update running animation based on whether player is moving        
        animator.SetBool("isRunning", motionVector != Vector2.zero);

        // Character flip when moving left or right horizontally
        if(motionVector.x != 0)
        {
            Vector3 scale = graphics.localScale;
            scale.x = motionVector.x > 0 ? 1 : -1;
            graphics.localScale = scale;
        }

        /*
            Code below is to check if Pick Up animation is playing.
            This is so when the player plays this animation, the player
            is unable to use AWSD/arrow movement until animation is complete
        */
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        bool isBusy = state.IsName("Chef_Pickup");

        if (isBusy)
        {
            motionVector = Vector2.zero;
            animator.SetBool("isRunning", false);
            return;
        }

        // When player is standing, pressing 'Z' triggers pickup animation.
        if (Input.GetKeyDown(KeyCode.Z) && motionVector == Vector2.zero && !isBusy)
        {
            animator.SetTrigger("pickUpMovement");
        }

    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move(){
        rigidbody2d.velocity = motionVector * speed;
    }
}
