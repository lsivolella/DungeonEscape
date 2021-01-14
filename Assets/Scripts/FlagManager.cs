using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    // Configuration Parameters
    [SerializeField] AudioClip flagSound;

    // Cached References
    private GameSession myGameSession;
    private GameObject jumpFlag;
    private GameObject climbFlag;
    private GameObject startFlag;
    private GameObject mainMenuFlag;
    private GameObject playAgainFlag;
    private Cinemachine2DOneShotAudio myCinemachine2DOneShotAudio;
    private float flagSoundLength;

    // Cached States
    private bool isJumpFlagOn = false;
    private bool isClimbFlagOn = false;
    private bool isStartFlagOn = false;
    private bool isMainMenuFlagOn = true;
    private bool isPlayAgainFlagOn = true;

    private void Awake()
    {
        GetComponents();
        FindObjects();
    }

    private void GetComponents()
    {
        myCinemachine2DOneShotAudio = GetComponent<Cinemachine2DOneShotAudio>();
    }

    private void FindObjects()
    {
        myGameSession = FindObjectOfType<GameSession>();
        jumpFlag = GameObject.Find("Jump Flag");
        climbFlag = GameObject.Find("Climb Flag");
        startFlag = GameObject.Find("Start Flag");
        mainMenuFlag = GameObject.Find("Main Menu Flag");
        playAgainFlag = GameObject.Find("Play Again Flag");
    }

    private void Start()
    {
        flagSoundLength = flagSound.length;

        JumpFlagController();
        ClimbFlagController();
        StartFlagController();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayFlagSound();
        DisableFlag();
        if (gameObject.name == "Move Flag")
        {
            isJumpFlagOn = true;
            JumpFlagController();

        }
        else if (gameObject.name == "Jump Flag")
        {
            isClimbFlagOn = true;
            ClimbFlagController();
        }
        else if (gameObject.name == "Climb Flag")
        {
            isStartFlagOn = true;
            StartFlagController();
        }
        else if (gameObject.name == "Start Flag")
        {
            StartCoroutine(myGameSession.StartGame(flagSoundLength));
        }
        else if (gameObject.name == "Play Again Flag")
        {
            isMainMenuFlagOn = false;
            MainMenuFlagController();
            StartCoroutine(myGameSession.StartGame(flagSoundLength));
        }
        else if (gameObject.name == "Main Menu Flag")
        {
            isPlayAgainFlagOn = false;
            PlayAgainFlagController();
            StartCoroutine(myGameSession.MainMenu(flagSoundLength));
        }
    }

    private void JumpFlagController()
    {
        if (jumpFlag)
        {
            if (isJumpFlagOn)
            {
                jumpFlag.GetComponent<SpriteRenderer>().enabled = true;
                jumpFlag.GetComponent<PolygonCollider2D>().enabled = true;
                jumpFlag.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                jumpFlag.GetComponent<SpriteRenderer>().enabled = false;
                jumpFlag.GetComponent<PolygonCollider2D>().enabled = false;
                jumpFlag.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private void ClimbFlagController()
    {
        if (climbFlag)
        {
            if (isClimbFlagOn)
            {
                climbFlag.GetComponent<SpriteRenderer>().enabled = true;
                climbFlag.GetComponent<PolygonCollider2D>().enabled = true;
                climbFlag.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                climbFlag.GetComponent<SpriteRenderer>().enabled = false;
                climbFlag.GetComponent<PolygonCollider2D>().enabled = false;
                climbFlag.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private void StartFlagController()
    {
        if (startFlag)
        {
            if (isStartFlagOn)
            {
                startFlag.GetComponent<SpriteRenderer>().enabled = true;
                startFlag.GetComponent<PolygonCollider2D>().enabled = true;
                startFlag.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                startFlag.GetComponent<SpriteRenderer>().enabled = false;
                startFlag.GetComponent<PolygonCollider2D>().enabled = false;
                startFlag.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private void MainMenuFlagController()
    {
        if (mainMenuFlag)
        {
            if (isMainMenuFlagOn)
            {
                mainMenuFlag.GetComponent<SpriteRenderer>().enabled = true;
                mainMenuFlag.GetComponent<PolygonCollider2D>().enabled = true;
                mainMenuFlag.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                mainMenuFlag.GetComponent<SpriteRenderer>().enabled = false;
                mainMenuFlag.GetComponent<PolygonCollider2D>().enabled = false;
                mainMenuFlag.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private void PlayAgainFlagController()
    {
        if (playAgainFlag)
        {
            if (isPlayAgainFlagOn)
            {
                playAgainFlag.GetComponent<SpriteRenderer>().enabled = true;
                playAgainFlag.GetComponent<PolygonCollider2D>().enabled = true;
                playAgainFlag.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                playAgainFlag.GetComponent<SpriteRenderer>().enabled = false;
                playAgainFlag.GetComponent<PolygonCollider2D>().enabled = false;
                playAgainFlag.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private void DisableFlag()
    {
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void PlayFlagSound()
    {
        myCinemachine2DOneShotAudio.ForcePlay2DClip(flagSound, Camera.main.transform.position);
    }


    //// Use this code to Deactivate all children from parent Object
    // *need to cache:* private Transform parent;
    // /need to initialize:* parent = gameObject.transform;
    //for (int i = 0; i < parent.transform.childCount; i++)
    //{
    //    var child = parent.transform.GetChild(i).gameObject;
    //    if (child != null)
    //        child.SetActive(false);
    //}
}
