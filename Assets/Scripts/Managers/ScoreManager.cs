using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI DefeatVictoryScoreText;
    int score;
    [SerializeField] float fadeTime = 0.5f;
    [SerializeField] float maximumScale = 5f;

    public static ScoreManager Instance;
    [HideInInspector] public bool canScoreGrow = true;
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

    public void AddScore(int scoreToAdd)
    {
        score += (scoreToAdd + ComboManager.Instance.GetCombo());
        scoreText.text = score.ToString();
        if (!canScoreGrow)
            return;
        scoreText.DOColor(Color.red, 0.01f);
        scoreText.transform.DOScale(maximumScale, 0.01f);

        scoreText.DOColor(Color.white, fadeTime);
        scoreText.transform.DOScale(1, fadeTime);
    }

    public void SeeScore()
    {
        DefeatVictoryScoreText.text = score.ToString();
    }
}
