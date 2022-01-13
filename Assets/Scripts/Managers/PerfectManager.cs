using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class PerfectManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI perfectText;

    [SerializeField] string[] perfectsWords;
    [SerializeField] Color color;
    [HideInInspector] public bool canPerfect = true;

    public static PerfectManager Instance;
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

    public void SpawnText()
    {
        if (!canPerfect)
            return;

        if (AudioReaction.Instance.GetDropValue() < 2)
            return;

        perfectText.transform.DOScale(1.7f, .01f);
        perfectText.DOColor(Color.red, .01f);

        int random = Random.Range(0, perfectsWords.Length);
        perfectText.text = perfectsWords[random];

        perfectText.transform.DOScale(1, .5f);
        perfectText.DOColor(color, .5f);
    }
}
