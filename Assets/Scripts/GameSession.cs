using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;

public class GameSession : MonoBehaviour
{
    // Configuration Parameters
    [Header("Seriealized Scenes")]
    [SerializeField] String mainMenuScene;
    [SerializeField] String firstLevelScene;
    [SerializeField] String gameOverScene;
    [Header("Game Session")]
    [SerializeField] float levelLoadDelay = 1f;
    [Header("Player Lives")]
    [SerializeField] TextMeshProUGUI playerLivesUI;
    [SerializeField] int playerLives = 1;

    // Cached Variables
    private GameSession[] myGameSession;
    private int currentScene;
    private int startingLives;

    private void Awake()
    {
        FindObjectsOfType();
        SetUpSingleton();
    }

    private void FindObjectsOfType()
    {
        myGameSession = FindObjectsOfType<GameSession>();
    }

    private void SetUpSingleton()
    {
        if (myGameSession.Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        startingLives = playerLives;
        UpdateLivesUI();
        ManageUICanvas();
    }

    private void UpdateLivesUI()
    {
        playerLivesUI.text = playerLives.ToString();
    }

    private void ManageUICanvas()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RestartScene();
    }

    public int GetCurrentScene()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;

        return currentScene;
    }

    private void RestartScene()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(GetCurrentScene());
            PlayerHealth.isAlive = true;
        }
    }

    public IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(levelLoadDelay);
        SceneManager.LoadScene(GetCurrentScene() + 1);
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            GameOver();
        }
    }

    private void TakeLife()
    {
        playerLives--;
        UpdateLivesUI();
        StartCoroutine(ReloadScene());
    }


    private IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(1);
        PlayerHealth.isAlive = true;
        SceneManager.LoadScene(GetCurrentScene());
    }

    private void GameOver()
    {
        playerLives--;
        UpdateLivesUI();
        StartCoroutine(LoadGameOver());
    }

    private IEnumerator LoadGameOver()
    {
        yield return new WaitForSeconds(1);
        PlayerHealth.isAlive = true;
        SceneManager.LoadScene(gameOverScene);
    }

    public IEnumerator MainMenu(float loadDelay)
    {
        yield return new WaitForSeconds(loadDelay);
        SceneManager.LoadScene(mainMenuScene);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    public IEnumerator StartGame(float loadDelay)
    {
        yield return new WaitForSeconds(loadDelay);
        playerLives = startingLives;
        UpdateLivesUI();
        SceneManager.LoadScene(firstLevelScene);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }
}
