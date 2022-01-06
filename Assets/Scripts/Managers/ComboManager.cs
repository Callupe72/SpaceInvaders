using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textCombo;
    [SerializeField] float cooldownBeforeEndCombo = 3f;
    [SerializeField] float animationTime = .5f;
    //[SerializeField] Image cooldownImg;
    Color colorFillAmount =  new Color(0,0,0,0);
    int combo;
    float timeSpent;
    bool playAnim;

    public static ComboManager Instance;
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

    void Start()
    {
        EnableText(false);
    }

    void Update()
    {
        if (playAnim)
        {
            textCombo.transform.DOScale(0, cooldownBeforeEndCombo);
            //cooldownImg.DOFillAmount(0, cooldownBeforeEndCombo);
            //cooldownImg.DOColor(colorFillAmount, cooldownBeforeEndCombo);
            textCombo.DOColor(colorFillAmount, cooldownBeforeEndCombo);
            timeSpent += Time.deltaTime;
            if (timeSpent > cooldownBeforeEndCombo)
            {
                ResetCombo();
            }
        }

    }

    public void AddCombo()
    {
        EnableText(true);
        timeSpent = 0;
        combo++;
        textCombo.transform.DOScale(3, .001f);
        textCombo.DOColor(Color.red, .001f);
        //cooldownImg.DOColor(Color.red, .001f);
        //cooldownImg.DOFillAmount(1, .001f);
        playAnim = true;
        SetText();
    }

    public void ResetCombo()
    {
        EnableText(false);
        combo = 0;
        playAnim = false;
        SetText();
    }

    void SetText()
    {
        textCombo.text = "x" + combo.ToString();
    }

    void EnableText(bool isEnable)
    {
        textCombo.gameObject.SetActive(isEnable);
    }

    public int GetCombo()
    {
        return combo;
    }

}
