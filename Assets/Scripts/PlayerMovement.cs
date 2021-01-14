using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerMovement : MonoBehaviour
{
    // Configuration Parameters
    [Header("Player Movement")]
    [SerializeField] Collider2D bodyCollider;
    [SerializeField] Collider2D feetCollider;
    [SerializeField] AudioClip playerJumpSound;
    [SerializeField] float runningSpeed = 1f;
    [SerializeField] float jumpingSpeed = 1f;
    [SerializeField] float fallMultiplier = 1f;
    [SerializeField] float lowJumpMultiplier = 1f;
    [SerializeField] float climbingSpeed = 1f;

    // Cached States
    private bool onPlataform = false;

    // Cached Variables;
    private Vector2 movementDirection;
    private float defaultGravityScale;
    private Transform originalParent;

    // Cached References
    private Animator myAnimator;
    private Rigidbody2D myRigidbody2D;
    private Cinemachine2DOneShotAudio myCinemachine2DOneShotAudio;

    // Start is called before the first frame update
    void Start()
    {
        GetComponents();
        GetGravityScale();

        originalParent = transform.parent;
    }

    private void GetComponents()
    {
        myAnimator = GetComponent<Animator>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myCinemachine2DOneShotAudio = GetComponent<Cinemachine2DOneShotAudio>();
    }

    private void GetGravityScale()
    {
        defaultGravityScale = myRigidbody2D.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerHealth.isAlive) { return; }

        PlayerJumping();
        ControllDownfallSpeed();
        PlayerClimbing();
        PlayerRunning();
    }

    private void PlayerJumping()
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && !feetCollider.IsTouchingLayers(LayerMask.GetMask("Plataform"))) { return; }

        if (Input.GetButtonDown("Jump"))
        {
            myCinemachine2DOneShotAudio.ForcePlay2DClip(playerJumpSound, Camera.main.transform.position);
            myRigidbody2D.velocity += Vector2.up * jumpingSpeed;
        }
    }

    private void ControllDownfallSpeed()
    {
        // If player is falling this line will make him fall much faster, eliminating the floating in mid-air sensation.
        if (myRigidbody2D.velocity.y < 0)
        {
            myRigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        // If the player releases the Jump button before it has ended he will then begin to fall (half-jump).
        else if (myRigidbody2D.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            myRigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void PlayerClimbing()
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myAnimator.SetBool("isClimbing", false);
            myRigidbody2D.gravityScale = defaultGravityScale;
            return;
        }

        movementDirection.y = Input.GetAxisRaw("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidbody2D.velocity.x, movementDirection.y * climbingSpeed);
        myRigidbody2D.velocity = climbVelocity;

        myRigidbody2D.gravityScale = 0;

        bool hasVerticalSpeed = Mathf.Abs(movementDirection.y) > Mathf.Epsilon;

        myAnimator.SetBool("isClimbing", hasVerticalSpeed);
    }

    private void PlayerRunning()
    {
        movementDirection.x = Input.GetAxisRaw("Horizontal");

        Vector2 playerVelocity = new Vector2(movementDirection.x * runningSpeed, myRigidbody2D.velocity.y);
        myRigidbody2D.velocity = playerVelocity;

        FlipSprite();

        bool isMovingHorizontally = Mathf.Abs(movementDirection.x) > Mathf.Epsilon;

        if (isMovingHorizontally)
        {
            myAnimator.SetBool("isRunning", true);
        }
        else
        {
            myAnimator.SetBool("isRunning", false);
        }
    }

    private void FlipSprite()
    {
        bool isMovingHorizontally = Mathf.Abs(movementDirection.x) > Mathf.Epsilon;
        if (isMovingHorizontally)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody2D.velocity.x), 1f);
        }
    }


    // Makes Player Object child to plataforms when Player is in contact with them.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        MakePlayerChildToPlataform(collision);
    }

    private void MakePlayerChildToPlataform(Collision2D collision)
    {
        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Plataform")) && !onPlataform)
        {
            transform.parent = collision.transform;
            onPlataform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        UnmakePlayerChildToPlataform();
    }

    private void UnmakePlayerChildToPlataform()
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Plataform")) && onPlataform)
        {
            transform.parent = originalParent;
            onPlataform = false;
        }
    }
}
