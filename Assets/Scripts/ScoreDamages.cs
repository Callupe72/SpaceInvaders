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
    Vector3 lastPos;
    public void SetText(int damages)
    {
        text.text = damages.ToString();
        StartCoroutine(TimeBeforeFadeOut());
    }

    IEnumerator TimeBeforeFadeOut()
    {
        yield return new WaitForSeconds(timeBeforeDisapear);
        canFadeOut = true;
        lastPos = transform.position;
        lastPos = new Vector3(lastPos.x, lastPos.y + 1.5f, lastPos.z);
        yield return new WaitForSeconds(fadeOutTime);
        Destroy(gameObject);
    }

    void Update()
    {
        if (canFadeOut)
        {
            text.DOColor(Color.white, fadeOutTime);
            text.transform.DOScale(0.5f, fadeOutTime);
            text.transform.DOMove(lastPos, fadeOutTime);
        }
    }
}
