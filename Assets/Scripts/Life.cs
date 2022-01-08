using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    [SerializeField] Image[] imgs;
    List<Color> imgsColor = new List<Color>();
    List<float> imgsSize = new List<float>();
    [SerializeField] Color hasLife;
    [SerializeField] Color hasNotLife;
    bool canGrow;
    [SerializeField] float growTime = 1f;
    float currentrGrowTime;

    void Start()
    {
        for (int i = 0; i < imgs.Length; i++)
        {
            imgsColor.Add(Color.red);
            imgsSize.Add(1);
        }
        SetColorLife(3);
    }

    public void SetColorLife(int playerLife)
    {

        playerLife--;

        for (int i = 0; i < imgs.Length; i++)
        {
            if (i <= playerLife)
            {
                imgsColor[i] = hasLife;
                imgsSize[i] = 1;
            }
            else
            {
                imgsColor[i] = hasNotLife;
                imgsSize[i] = 0.8f;
                imgs[i].transform.DOScale(1, .01f);
            }

            imgs[i].DOColor(Color.white, .01f);
        }
        transform.DOScale(5, .01f);
        canGrow = true;
    }

    void Update()
    {
        transform.Rotate(0, 0, 0.25f);

        if (canGrow)
        {
            transform.DOScale(2, growTime);
            currentrGrowTime += Time.deltaTime;

            for (int i = 0; i < imgs.Length; i++)
            {
                imgs[i].DOColor(imgsColor[i], growTime);
                if(imgsSize[i] != 1)
                {
                    imgs[i].transform.DOScale(imgsSize[i], growTime);
                }
            }
            if (currentrGrowTime > growTime)
            {
                canGrow = false;
            }
        }
    }
}
