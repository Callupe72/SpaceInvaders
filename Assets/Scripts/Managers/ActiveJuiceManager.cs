using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveJuiceManager : MonoBehaviour
{

    [SerializeField] bool debug;
    [SerializeField] Transform juicinessParent;
    [SerializeField] GameObject juicinessToSpawn;


    [System.Serializable]
    public struct ActiveJuiceValues
    {
        [HideInInspector] public string effectToActive;
        public enum AllEffect
        {
            ActiveEveryhing,
            PostProcess,
            Bloom,
            ShipExplostion,
            Sound,
            Music,
            ShakeCamera,
            XpBar,
            XpText,
            Animations,
        }

        [Header("Essentials")]
        public AllEffect whichEffect;

        public enum HowToModify
        {
            Toggle,
            Slider,
            Title,
        }

        public HowToModify whatToUse;

        [Header("IF Slider")]

        public float minValue;
        public float maxValue;
        public bool wholeNumbers;

        [HideInInspector] public JuicinessSpawner juicinessSpawner;

        [Header("IF Title")]

        public string titleName;

    }
    [Header("ToModify")]

    [SerializeField] ActiveJuiceValues[] activeJuices;

    public static ActiveJuiceManager Instance;

    [SerializeField] bool generateButtons;
    [SerializeField] bool destroyAllButtons;

    void OnValidate()
    {
        if (!debug)
            return;
        if (generateButtons)
        {
            generateButtons = false;
            GenerateButtons();
        }
        if (destroyAllButtons)
        {
            destroyAllButtons = false;
            DestroyButtons();
        }

        ChangeName();
    }

    void DestroyButtons()
    {
        for (int i = 0; i < juicinessParent.transform.childCount; i++)
        {
            GameObject goToDestroy = juicinessParent.transform.GetChild(i).gameObject;
            UnityEditor.EditorApplication.delayCall += () =>
            {
                DestroyImmediate(goToDestroy);
            };
        }
    }

    void Start()
    {
        SetValuesOnEnable();
        for (int i = 0; i < activeJuices.Length; i++)
        {
            activeJuices[i].juicinessSpawner.SaveResetValue();
        }
    }

    void ChangeName()
    {
        for (int i = 0; i < activeJuices.Length; i++)
        {
            activeJuices[i].effectToActive = activeJuices[i].whichEffect.ToString();
        }
    }

    void GenerateButtons()
    {
        for (int i = 0; i < activeJuices.Length; i++)
        {
            Transform juicinessTransform = Instantiate(juicinessToSpawn).transform;
            TextMeshProUGUI juicinessText = juicinessTransform.GetComponentInChildren<TextMeshProUGUI>();
            juicinessText.text = activeJuices[i].effectToActive;
            juicinessText.transform.name = "Juice : " + activeJuices[i].effectToActive + "Txt";
            juicinessTransform.transform.parent = juicinessParent;

            juicinessTransform.transform.name = "Juciness" + activeJuices[i].effectToActive + activeJuices[i].whatToUse;

            switch (activeJuices[i].whatToUse)
            {
                case ActiveJuiceValues.HowToModify.Toggle:
                    Debug.Log("true");
                    juicinessTransform.GetComponent<JuicinessSpawner>().toggle.gameObject.SetActive(true);
                    break;
                case ActiveJuiceValues.HowToModify.Slider:
                    juicinessTransform.GetComponent<JuicinessSpawner>().slider.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
            activeJuices[i].juicinessSpawner = juicinessTransform.GetComponent<JuicinessSpawner>();
            activeJuices[i].juicinessSpawner.thisEffect = activeJuices[i].whichEffect;
            activeJuices[i].juicinessSpawner.modifyValue = activeJuices[i].whatToUse;
            activeJuices[i].juicinessSpawner.slider.minValue = activeJuices[i].minValue;
            activeJuices[i].juicinessSpawner.slider.maxValue = activeJuices[i].maxValue;
            activeJuices[i].juicinessSpawner.slider.wholeNumbers = activeJuices[i].wholeNumbers;

            if (activeJuices[i].whatToUse == ActiveJuiceValues.HowToModify.Title)
            {
                juicinessText.text = activeJuices[i].titleName;
                activeJuices[i].juicinessSpawner.name = "Title : " + activeJuices[i].titleName;
                juicinessText.fontSize = 50;
                juicinessText.fontStyle = FontStyles.Bold;
                juicinessTransform.GetComponentInChildren<Button>().gameObject.SetActive(false);
            }
        }

        float sizeY = juicinessParent.transform.GetComponent<GridLayoutGroup>().cellSize.y * juicinessParent.transform.childCount;
        juicinessParent.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, sizeY);
    }

    public float GetValueFloat(ActiveJuiceValues.AllEffect whichEffect)
    {
        for (int i = 0; i < activeJuices.Length; i++)
        {
            if (activeJuices[i].whichEffect == whichEffect)
            {
                return activeJuices[i].juicinessSpawner.slider.value;
            }
        }

        Debug.Log("No juiciness found with " + whichEffect.ToString());
        return 0;
    }

    public bool GetValueBool(ActiveJuiceValues.AllEffect whichEffect)
    {
        for (int i = 0; i < activeJuices.Length; i++)
        {
            if (activeJuices[i].whichEffect == whichEffect)
            {
                return activeJuices[i].juicinessSpawner.toggle.isOn;
            }
        }
        Debug.Log("No juiciness found with " + whichEffect.ToString());
        return false;
    }

    public void SetValuesOnEnable()
    {
        for (int i = 0; i < activeJuices.Length; i++)
        {
            activeJuices[i].juicinessSpawner.SetButtonTo();
        }
    }

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

    public void ActiveAllToggle(bool isOn)
    {
        for (int i = 0; i < activeJuices.Length; i++)
        {
            if (activeJuices[i].whatToUse == ActiveJuiceValues.HowToModify.Toggle)
            {
                if (activeJuices[i].whichEffect != ActiveJuiceValues.AllEffect.ActiveEveryhing)
                {

                    activeJuices[i].juicinessSpawner.toggle.isOn = isOn;
                    if (isOn)
                    {
                        activeJuices[i].juicinessSpawner.ChangeValue();
                    }
                }
            }
        }
    }

    public void ChangeActiveAllToggle(bool isOn)
    {
        for (int i = 0; i < activeJuices.Length; i++)
        {
            if (activeJuices[i].whichEffect == ActiveJuiceValues.AllEffect.ActiveEveryhing && activeJuices[i].whatToUse == ActiveJuiceValues.HowToModify.Toggle)
            {
                if (!isOn)
                {
                    activeJuices[i].juicinessSpawner.toggle.SetIsOnWithoutNotify(false);
                    return;
                }
                else
                {
                    for (int j = 0; j < activeJuices.Length; j++)
                    {
                        if (activeJuices[j].whatToUse == ActiveJuiceValues.HowToModify.Toggle && activeJuices[j].whichEffect != ActiveJuiceValues.AllEffect.ActiveEveryhing)
                        {
                            if (activeJuices[j].juicinessSpawner.toggle.isOn == false)
                            {
                                activeJuices[j].juicinessSpawner.toggle.SetIsOnWithoutNotify(false);
                                return;
                            }
                        }
                    }

                    activeJuices[i].juicinessSpawner.toggle.SetIsOnWithoutNotify(true);
                }
            }
        }
    }

    public void ResetAllButtons()
    {
        for (int i = 0; i < activeJuices.Length; i++)
        {
            activeJuices[i].juicinessSpawner.ResetButtons();
        }
    }
}

