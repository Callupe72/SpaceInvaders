using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class XPManager : MonoBehaviour
{
    [SerializeField] Image xpBarBack;
    [SerializeField] Image xpBar;
    [SerializeField] GameObject xpText;
    [SerializeField] Transform xpTextParent;

    float playerXp;
    float xpBeforeNextLvl = 2000;

    int level;

    bool canFollow;
    float speed = 10f;
    float timeBeforeFollow = .5f;

    public static XPManager Instance;
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
        if (canFollow)
        {
            xpBar.fillAmount = Mathf.Lerp(xpBar.fillAmount, xpBarBack.fillAmount, Time.deltaTime * speed);
            if (xpBar.fillAmount >= 0.99)
            {
                LevelUp();
            }
            if (xpBar.fillAmount >= xpBarBack.fillAmount-0.00005)
            {
                canFollow = false;
            }
        }
    }

    public void AddXP(int xp)
    {
        playerXp += xp;
        TextMeshProUGUI text = Instantiate(xpText, xpTextParent).GetComponent<TextMeshProUGUI>();
        text.rectTransform.anchoredPosition = new Vector2(xpBarBack.fillAmount * xpBar.rectTransform.sizeDelta.x - 100 - xpBar.rectTransform.sizeDelta.x / 3, -40);
        text.text = "+" + xp + " xp";
        StartCoroutine(WaitBeforeFollow(xp));
    }

    IEnumerator WaitBeforeFollow(int xp)
    {
        //White Bar
        xpBarBack.fillAmount = playerXp / xpBeforeNextLvl;
        yield return new WaitForSeconds(timeBeforeFollow);

        //Red Bar
        canFollow = true;
    }

    void LevelUp()
    {
        level++;
        xpBar.fillAmount = 0;
        xpBarBack.fillAmount = 0;
    }

}
