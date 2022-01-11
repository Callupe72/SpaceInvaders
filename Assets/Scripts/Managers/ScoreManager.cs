using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    int score;
    bool canFade;
    [SerializeField] float fadeTime = 0.5f;
    [SerializeField] float maximumScale = 5f;

    public static ScoreManager Instance;

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

    void Update()
    {
        if (canFade)
        {
            scoreText.DOColor(Color.white, fadeTime);
            scoreText.transform.DOScale(1, fadeTime);
        } 
    }

    public void AddScore(int scoreToAdd)
    {
        canFade = false;
        score += (scoreToAdd + ComboManager.Instance.GetCombo());
        scoreText.text = score.ToString();
        scoreText.DOColor(Color.red, 0.01f);
        scoreText.transform.DOScale(maximumScale, 0.01f);
        canFade = true;
    }
}
