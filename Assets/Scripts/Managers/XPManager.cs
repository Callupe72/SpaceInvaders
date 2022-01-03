using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class XPManager : MonoBehaviour
{
    [SerializeField] Image xpBarBack;
    [SerializeField] Image xpBar;

    float playerXp;
    float xpBeforeNextLvl = 100;

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
            Debug.Log(xpBar.fillAmount);
            if (xpBar.fillAmount >= 0.99)
            {
                LevelUp();
            }
            if (xpBar.fillAmount >= xpBarBack.fillAmount-0.05f)
            {
                canFollow = false;
            }
        }
    }

    public void AddXP(int xp)
    {
        playerXp += xp;
        StartCoroutine(WaitBeforeFollow(xp));
    }

    IEnumerator WaitBeforeFollow(int xp)
    {
        //Barre blanche
        xpBarBack.fillAmount = playerXp / xpBeforeNextLvl;
        yield return new WaitForSeconds(timeBeforeFollow);

        //Barre rouge
        canFollow = true;
    }

    void LevelUp()
    {
        level++;
        xpBar.fillAmount = 0;
        xpBarBack.fillAmount = 0;
    }

}
