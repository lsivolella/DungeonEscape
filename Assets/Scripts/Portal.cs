using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // Serialized Parameters
    [SerializeField] AudioClip portalSound;

    // Cached References
    private GameSession myGameSession;
    private Cinemachine2DOneShotAudio myCinemachine2DOneShotAudio;

    // Start is called before the first frame update
    void Start()
    {
        GetComponents();
        FindObjectsOfType();
    }

    private void GetComponents()
    {
        myCinemachine2DOneShotAudio = GetComponent<Cinemachine2DOneShotAudio>();
    }

    private void FindObjectsOfType()
    {
        myGameSession = FindObjectOfType<GameSession>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //HideElements();
        myCinemachine2DOneShotAudio.ForcePlay2DClip(portalSound, Camera.main.transform.position);
        StartCoroutine(myGameSession.LoadNextScene());
    }

    private void HideElements()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<ParticleSystem>().Stop();
    }
}
