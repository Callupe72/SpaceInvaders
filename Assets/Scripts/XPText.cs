using DG.Tweening;
using System.Collections;
using UnityEngine;

public class XPText : MonoBehaviour
{
    [SerializeField] RectTransform rect;
    [SerializeField] float timeBeforeDisapear = 0.5f;
    bool canShowOff = false;

    void Start()
    {
        StartCoroutine(WaitBeforeShowOff());
    }

    IEnumerator WaitBeforeShowOff()
    {
        yield return new WaitForSeconds(.5f);
        canShowOff = true;
    }

    void Update()
    {
        rect.DOAnchorPosY(rect.anchoredPosition.y + 2, timeBeforeDisapear);
        if (canShowOff)
        {
            rect.DOScale(Vector2.zero, timeBeforeDisapear);
            if (rect.localScale.x <= 0.05)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
