using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.EventSystems;

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
    Pickup pickup;
    Combat combat;

    public Gameloop gameloop;
    public AudioSource knifeSwoosh;
    public AudioSource footstepSound;
    [SerializeField] float footstepInterval = 0.4f;
    private float footstepTimer = 0f;

    private bool attacking;
    private bool dying;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        pickup = GetComponent<Pickup>();
        combat = GetComponent<Combat>();
        GetComponent<Health>().OnDeath += Died; // connect to health's death event
    }

    /*
       Get raw AWSD/arrow key input and normalize so diagonal
       movement is not faster than horizontal/vertical movement.
    */
    private void SetMotionVector()
    {
        motionVector = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        // Update running animation based on whether player is moving        
        animator.SetBool("isRunning", !IsStandingStill());
    }

    // Character flip when moving left or right horizontally
    private void FlipCharacter()
    {
        if (motionVector.x != 0)
        {
            Vector3 scale = graphics.localScale;
            scale.x = motionVector.x > 0 ? 1 : -1;
            graphics.localScale = scale;
        }
    }

    // force player to stand still
    private void StandStill()
    {
        motionVector = Vector2.zero;
        animator.SetBool("isRunning", false);
    }

    // if player is standing still
    private bool IsStandingStill()
    {
        return motionVector == Vector2.zero;
    }

    private void Update()
    {
        if (dying) return;
        /*
            Code below is to check if Pick Up animation is playing.
            This is so when the player plays this animation, the player
            is unable to use AWSD/arrow movement until animation is complete
        */
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        bool isPickUpBusy = state.IsName("Chef_Pickup");
        bool isCombatBusy = state.IsName("Chef_Attack");
        bool isCombatMovementBusy = state.IsName("Chef_MoveAttack");

        if (isPickUpBusy)
        {
            StandStill();
            return;
        }

        SetMotionVector();
        HandleFootsteps();

        if (attacking) return;

        FlipCharacter();


        // When player is standing, pressing 'E' triggers pickup animation.
        if (Input.GetButtonDown("Pickup"))
        {
            animator.SetTrigger("pickUpMovement");
            pickup?.Take();
            StandStill();
        }

        // When player is standing, pressing "Space" bar trigers idle combat animation
        //if (Input.GetButtonDown("Attack"))
        //{
        //    Attack("combatIdleMovement");
        //}

        // When player is moving and press "Space" bar trigers moving combat animation.
        if (Input.GetButtonDown("Attack"))
        {
            Attack("combatMovement");
        }

    }

    // play attack animation
    private void Attack(string triggerName)
    {
        // if hovering over a UI element, dont attack
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // if not on attack cooldown
        if (combat.CanAttack())
        {
            knifeSwoosh.PlayOneShot(knifeSwoosh.clip);
            attacking = true;
            animator.SetTrigger(triggerName);
            combat.Attack();
        }
    }

    private void AttackEnd()
    {
        attacking = false;
        combat.AttackEnd();
    }

    // foot steps audio
    private void HandleFootsteps()
    {
        if (!IsStandingStill() && !dying)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                footstepSound.PlayOneShot(footstepSound.clip);
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }

    private void Died()
    {
        dying = true;
        StandStill();
        animator.SetTrigger("Die");
    }

    private void DieAnimationFinished()
    {
        gameloop.Died();
    }

    // set speed for the cheat menu
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move(){
        rigidbody2d.velocity = motionVector * speed;
    }
}
