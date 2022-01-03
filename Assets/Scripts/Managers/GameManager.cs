using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool isPause = false;
    [SerializeField] GameObject pauseMenu;

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            Pause();
        }
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
