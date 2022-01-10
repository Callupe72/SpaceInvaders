using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuWinLose : MonoBehaviour
{
    [SerializeField] GameObject mainMenu = null;
    [SerializeField] GameObject optionPanel = null;
    [SerializeField] TMP_Text text = null;
    [HideInInspector] public string Title = "";

    public void Retry()
    {
        SceneManager.LoadScene(1);
    }

    public void Credits()
    {
        mainMenu.SetActive(false);
        optionPanel.SetActive(true);
    }

    public void ReturnMenu()
    {
        mainMenu.SetActive(true);
        optionPanel.SetActive(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void OnEnable()
    {
        text.text = Title;
    }
}
