using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Enemy Movement")]
    [SerializeField] Collider2D bodyCollider;
    [SerializeField] Collider2D groundCheckerCollider;
    [SerializeField] float runningSpeed = 1f;

    // Cached States
    private bool isAlive = true;

    // Cached Variables;
    private Vector2 movementDirection;

    // Cached References
    private Animator myAnimator;
    private Rigidbody2D myRigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        GetComponents();
    }

    private void GetComponents()
    {
        myAnimator = GetComponent<Animator>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFacingRight())
        {
            myRigidbody2D.velocity = Vector2.right * runningSpeed;
        }
        else
        {
            myRigidbody2D.velocity = Vector2.left * runningSpeed;
        }

    }

    private bool IsFacingRight()
    {
        bool isFacingRight = transform.localScale.x > 0;
        return isFacingRight;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidbody2D.velocity.x)), 1f);
    }

}
