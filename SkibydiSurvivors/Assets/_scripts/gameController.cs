using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gameController : MonoBehaviour
{
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
    int level;

    [Header("Stopwatch")]
    public float timeLimit; // in seconds
    float stopwatchtime;
    public TextMeshProUGUI stopwatchDisplay;

    public bool isGameOver = false;

    [Header("UI")]
    public GameObject pauseScreen;

    private void Awake()
    {
        DisableScreens();
    }

    void Update()
    {
        switch (currentState)
        {
            case GameState.Gameplay:
                CheckForPauseAndResume();
                UpdateStopwatch();
                break;

            case GameState.Paused:
                CheckForPauseAndResume();
                break;

            case GameState.GameOver:
                if (!isGameOver)
                {
                    isGameOver = true;
                    Debug.Log("GAME IS OVER");
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

    void CheckForPauseAndResume()
    {

    }

    void DisableScreens()
    {
        pauseScreen.SetActive(false);
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
