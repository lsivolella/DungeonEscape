using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    // Serialized Parameters
    [SerializeField] AudioClip introMusicFile;
    [SerializeField] AudioClip[] gameMusicFiles;
    [SerializeField] AudioClip gameOverMusicFile;
    [SerializeField] AudioClip victoryMusicFile;

    // Cahced References
    private MusicPlayer[] myMusicPLayer;
    private AudioSource myAudioSource;

    // Cached Variables
    private int finishedCount;
    private int randomIndex;


    private void Awake()
    {
        GetComponents();
        FindObjectsOfType();
        SetUpSingleton();
    }

    private void GetComponents()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    private void FindObjectsOfType()
    {
        myMusicPLayer = FindObjectsOfType<MusicPlayer>();
    }


    private void SetUpSingleton()
    {
        if (myMusicPLayer.Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ManageMusicPlayer();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void ManageMusicPlayer()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            myAudioSource.clip = introMusicFile;
            myAudioSource.loop = true;
        }
        else if (SceneManager.GetActiveScene().buildIndex > 0 && SceneManager.GetActiveScene().buildIndex < 4)
        {   
            randomIndex = Random.Range(0, gameMusicFiles.Length);
            myAudioSource.clip = gameMusicFiles[randomIndex];
            myAudioSource.loop = false;

        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            myAudioSource.clip = victoryMusicFile;
            myAudioSource.loop = false;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            myAudioSource.clip = gameOverMusicFile;
            myAudioSource.loop = false;
        }
        myAudioSource.Play();
    }

    private void Update()
    {
        if (!myAudioSource.isPlaying && SceneManager.GetActiveScene().buildIndex > 0 && SceneManager.GetActiveScene().buildIndex < 4)
        {
            finishedCount++;
            if (finishedCount > 1)
            {
                ManageMusicPlayer();
            }
        }
        else
        {
            finishedCount = 0;
        }
    }
}
