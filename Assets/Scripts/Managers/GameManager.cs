using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] RectTransform juicinessArray;
    public enum GameState
    {
        InGame,
        InPause,
        InJuicinessMenu,
        Victory,
        Defeat,
    }

    public GameState currentGameState;

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
        if (currentGameState == GameState.InPause)
            Pause();
        else if (currentGameState == GameState.InJuicinessMenu)
            JuicinessMenu();
    }

    void Update()
    {
        if (currentGameState != GameState.InJuicinessMenu)
        {
            if (Input.GetButtonDown("Pause"))
            {
                Pause();
            }
        }

        if (currentGameState != GameState.InPause)
        {
            if (Input.GetButtonDown("JuicinessMenu"))
            {
                JuicinessMenu();
            }
        }
    }

    void JuicinessMenu()
    {
        currentGameState = currentGameState == GameState.InJuicinessMenu ? GameState.InGame : GameState.InJuicinessMenu;

        ActiveJuiceManager.Instance.menuIsActive = currentGameState == GameState.InJuicinessMenu;

        if (currentGameState == GameState.InJuicinessMenu)
        {
            juicinessArray.anchoredPosition = new Vector2(-50, 0);
        }
        else
        {
            juicinessArray.anchoredPosition = new Vector2(99999, 0);
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
        currentGameState = currentGameState == GameState.InPause ? GameState.InGame : GameState.InPause;

        bool isInPause = currentGameState == GameState.InPause;
        pauseMenu.SetActive(isInPause);
        Time.timeScale = Convert.ToInt32(!isInPause);

        if (currentGameState == GameState.InPause)
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
        AudioReaction.Instance.PauseMusic();
    }
    void GameIsPlayed()
    {
        AudioReaction.Instance.PlayMusic();
    }
}
