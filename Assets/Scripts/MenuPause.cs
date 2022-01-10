using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    [SerializeField] GameObject mainMenu = null;
    [SerializeField] GameObject creditPanel = null;

    public void Continue()
    {
        GameManager.Instance.Pause();
    }

    public void Credits()
    {
        mainMenu.SetActive(false);
        creditPanel.SetActive(true);
    }

    public void ReturnMenu()
    {
        mainMenu.SetActive(true);
        creditPanel.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
