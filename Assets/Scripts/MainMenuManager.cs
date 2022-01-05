using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject mainPanel;
    [SerializeField]
    GameObject optionPanel;
    [SerializeField]
    GameObject creditPanel;

    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void OptionButton()
    {
        mainPanel.SetActive(false);
        optionPanel.SetActive(true);
    }

    public void CreditButton()
    {
        mainPanel.SetActive(false);
        creditPanel.SetActive(true);
    }

    public void ReturnButton()
    {
        mainPanel.SetActive(true);
        optionPanel.SetActive(false);
        creditPanel.SetActive(false);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
