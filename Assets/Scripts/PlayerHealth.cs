using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Configuration Parameters
    [Header("Player Health")]
    [SerializeField] Collider2D bodyCollider;
    [SerializeField] AudioClip playerDeathSound;
    [SerializeField] float deathKnockbackForce = 1f;

    // Cached States
    public static bool isAlive = true;

    // Cached References
    private Animator myAnimator;
    private Rigidbody2D myRigidbody2D;
    private GameSession myGameSession;
    private Cinemachine2DOneShotAudio myCinemachine2DOneShotAudio;

    // Cached Variables
    Vector2 directionOfLaunch;

    // Start is called before the first frame update
    void Start()
    {
        GetComponents();
        FindObjectsOfType();
    }

    private void GetComponents()
    {
        myAnimator = GetComponent<Animator>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myCinemachine2DOneShotAudio = GetComponent<Cinemachine2DOneShotAudio>();
    }

    private void FindObjectsOfType()
    {
        myGameSession = FindObjectOfType<GameSession>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAlive) { return; }
        else
        {
            directionOfLaunch = transform.position - collision.transform.position;
        }
    }

    private void Update()
    {
        KillPlayer();
        DrawnPlayer();
    }

    private void KillPlayer()
    {
        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")) && isAlive)
        {
            myCinemachine2DOneShotAudio.ForcePlay2DClip(playerDeathSound, Camera.main.transform.position);
            myAnimator.SetTrigger("Die");
            Vector2 deathKnockback = new Vector2(Mathf.Sign(directionOfLaunch.x) * deathKnockbackForce, deathKnockbackForce * 2);
            myRigidbody2D.velocity = deathKnockback;
            StartCoroutine(ImmobilizePlayerCorpse());
            isAlive = false;
            myGameSession.ProcessPlayerDeath();
        }
    }

    private void DrawnPlayer()
    {
        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Water")) && isAlive)
        {
            myCinemachine2DOneShotAudio.ForcePlay2DClip(playerDeathSound, Camera.main.transform.position);
            myAnimator.SetTrigger("Die");
            myRigidbody2D.velocity = new Vector2(0, myRigidbody2D.velocity.y);
            isAlive = false;
            myGameSession.ProcessPlayerDeath();
        }
    }

    IEnumerator ImmobilizePlayerCorpse()
    {
        yield return new WaitForSeconds(3);
        myRigidbody2D.velocity = new Vector2(0f, 0f);
    }
}
