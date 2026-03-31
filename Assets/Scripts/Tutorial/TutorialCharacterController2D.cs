using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
public class TutorialCharacterController2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private Transform graphics;

    [Header("Tutorial Bounds")]
    [SerializeField] private SpriteRenderer tutorialBackground;
    [SerializeField] private float horizontalPadding = 0.3f;
    [SerializeField] private float minRoadY = -4.2f;
    [SerializeField] private float maxRoadY = -2.6f;

    [Header("Optional")]
    [SerializeField] private AudioSource knifeSwoosh;

    private Rigidbody2D rb;
    private Animator animator;
    private Pickup pickup;
    private Combat combat;
    private Health health;

    private Vector2 motionVector;
    private bool dying;

    public bool HasMoved { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        pickup = GetComponent<Pickup>();
        combat = GetComponent<Combat>();
        health = GetComponent<Health>();

        if (health != null)
        {
            health.OnDeath += Died;
        }

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (dying) return;

        HandleMovementInput();
        HandlePickupInput();
        HandleAttackInput();
    }

    private void FixedUpdate()
    {
        if (dying) return;

        Move();
        ClampToTutorialArea();
    }

    private void HandleMovementInput()
    {
        motionVector = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        if (motionVector != Vector2.zero)
        {
            HasMoved = true;
        }

        if (animator != null)
        {
            animator.SetBool("isRunning", motionVector != Vector2.zero);
        }

        if (graphics != null && motionVector.x != 0)
        {
            Vector3 scale = graphics.localScale;
            scale.x = motionVector.x > 0 ? 1f : -1f;
            graphics.localScale = scale;
        }
    }

    private void Move()
    {
        rb.velocity = motionVector * speed;
    }

    private void ClampToTutorialArea()
    {
        if (tutorialBackground == null) return;

        Bounds bounds = tutorialBackground.bounds;
        Vector2 pos = rb.position;

        float minX = bounds.min.x + horizontalPadding;
        float maxX = bounds.max.x - horizontalPadding;

        float clampedX = Mathf.Clamp(pos.x, minX, maxX);
        float clampedY = Mathf.Clamp(pos.y, minRoadY, maxRoadY);

        rb.position = new Vector2(clampedX, clampedY);

        Vector2 velocity = rb.velocity;

        if ((pos.x <= minX && velocity.x < 0f) || (pos.x >= maxX && velocity.x > 0f))
            velocity.x = 0f;

        if ((pos.y <= minRoadY && velocity.y < 0f) || (pos.y >= maxRoadY && velocity.y > 0f))
            velocity.y = 0f;

        rb.velocity = velocity;
    }

    private void HandlePickupInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (pickup != null)
            {
                if (animator != null)
                {
                    animator.SetTrigger("pickUpMovement");
                }

                pickup.Take();
            }
        }
    }

    private void HandleAttackInput()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        // Left click
        if (Input.GetMouseButtonDown(0))
        {
            if (combat != null && combat.CanAttack())
            {
                if (knifeSwoosh != null && knifeSwoosh.clip != null)
                {
                    knifeSwoosh.PlayOneShot(knifeSwoosh.clip);
                }

                if (animator != null)
                {
                    animator.SetTrigger("combatMovement");
                }

                combat.Attack();
            }
        }
    }

    // Keep this private if your animation event already calls it by name
    private void AttackEnd()
    {
        if (combat != null)
        {
            combat.AttackEnd();
        }
    }

    private void Died()
    {
        dying = true;
        motionVector = Vector2.zero;

        if (animator != null)
        {
            animator.SetBool("isRunning", false);
            animator.SetTrigger("Die");
        }
    }
}