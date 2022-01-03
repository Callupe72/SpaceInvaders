using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ScoreDamages : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float timeBeforeDisapear = 1f;
    [SerializeField] float fadeOutTime = 1f;
    bool canFadeOut = false;
    public void SetText(int damages)
    {
        text.text = damages.ToString();
        StartCoroutine(TimeBeforeFadeOut());
    }

    IEnumerator TimeBeforeFadeOut()
    {
        yield return new WaitForSeconds(timeBeforeDisapear);
        canFadeOut = true;
        yield return new WaitForSeconds(fadeOutTime);
        transform.gameObject.SetActive(false);
    }

    void Update()
    {
        if (canFadeOut)
        {
            text.DOColor(Color.white, fadeOutTime);
            text.transform.DOScale(0.5f, fadeOutTime);
        }
    }
}
