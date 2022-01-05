using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool isPause = false;
    [SerializeField] GameObject pauseMenu;

    public enum GameState
    {
        InGame,
        Victory,
        Defeat,
    }

    GameState currentGameState;

    public static GameManager Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        if (isPause)
            Pause();
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            Pause();
        }
    }

    public void ChangeGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        switch (currentGameState)
        {
            case GameState.InGame:
                break;
            case GameState.Victory:
                Victory();
                break;
            case GameState.Defeat:
                Defeat();
                break;
            default:
                break;
        }
    }

    void Victory()
    {
        Debug.Log("Victory");
    }
    void Defeat()
    {
        Debug.Log("Defeat");
    }

    void Pause()
    {
        isPause = !isPause;

        pauseMenu.SetActive(isPause);
        Time.timeScale = Convert.ToInt32(!isPause);

        if (isPause)
        {
            GameIsPaused();
        }
        else
        {
            GameIsPlayed();
        }
    }

    void GameIsPaused()
    {
    }
    void GameIsPlayed()
    {
    }
}
