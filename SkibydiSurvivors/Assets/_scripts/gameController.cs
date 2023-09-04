using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gameController : MonoBehaviour
{
    public static gameController instance;

    //states of game
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver
    }

    public GameState currentState;
    public GameState previosState;

    public static int eliminations;

    [Header("Stopwatch")]
    public float timeLimit; // in seconds
    float stopwatchtime;
    public TextMeshProUGUI stopwatchDisplay;

    public bool isGameOver = false;

    [Header("UI")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;

    //Current stat displays
    public TextMeshProUGUI currentHealthDisplay;
    public TextMeshProUGUI currentRecoveryDisplay;
    public TextMeshProUGUI currentMagnetDisplay;
    public TextMeshProUGUI currentMoveSpeedDisplay;
    public TextMeshProUGUI currentMightDisplay;

    private void Awake()
    {
        //Warning check to see if there is another singleton of this kind in the game
        if(instance== null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("EXTRA " + this + " DELETED");
        }

        DisableScreens();
    }

    void Update()
    {
        switch (currentState)
        {
            case GameState.Gameplay:
                UpdateStopwatch();
                break;

            case GameState.Paused:
                break;

            case GameState.GameOver:
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f;
                    Debug.Log("GAME IS OVER");
                    DisplayResults();
                }
                break;

            default:
                Debug.LogWarning("STATE DOES NOT EXIST");
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            previosState = currentState;
            ChangeState(GameState.Paused);
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
            Debug.Log("Game is paused");
        }
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            ChangeState(previosState);
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
            Debug.Log("Game is resumed");
        }
    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver);
    }

    void DisplayResults()
    {
        resultsScreen.SetActive(true);
    }

    void CheckForPauseAndResume()
    {
        if (currentState == GameState.Paused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    void DisableScreens()
    {
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
    }

    void UpdateStopwatch()
    {
        stopwatchtime += Time.deltaTime;

        UpdateStopwatchDisplay();

        if (stopwatchtime >= timeLimit)
        {
            GameOver();
        }
    }

    void UpdateStopwatchDisplay()
    {
        int minutes = Mathf.FloorToInt(stopwatchtime / 60);
        int seconds = Mathf.FloorToInt(stopwatchtime % 60);

        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
