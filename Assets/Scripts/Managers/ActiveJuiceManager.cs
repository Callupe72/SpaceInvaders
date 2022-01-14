using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActiveJuiceManager : MonoBehaviour
{
    bool canShow;
    [Header("Debug")]
    [SerializeField] bool generateButtons;

    [SerializeField] bool debug;
    [SerializeField] Transform juicinessParent;
    [SerializeField] GameObject juicinessToSpawn;
    [SerializeField] EventSystem eventSystem;

    [SerializeField] Transform juicinessTextParent;
    [SerializeField] GameObject juicinessTextInstance;
    public bool menuIsActive;
    [SerializeField] RectTransform juicinessArray;

    public bool AnimationIsOn = true;
    public bool ExplosionIsOn = true;
    public bool BarrelRollIsOn = true;
    public bool ZoomIsOn = true;
    public bool SlowmotionIsOn = true;
    public bool TrailsIsOn = true;

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
            Particles,
            Combo,
            BarrelRoll,
            SlowMotion,
            Zoom,
            Trails,
            PerfectText,
            TextDamages,
            EnemyStillAlive,
            Score,
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

        [Range(0, 9)] public int numpadToActive;

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

    void OnValidate()
    {
        if (generateButtons)
        {
            generateButtons = false;
            DestroyButtons();
            GenerateButtons();
        }

        if (!debug)
            return;
        ChangeName();
    }

    void DestroyButtons()
    {
        for (int i = 0; i < juicinessParent.transform.childCount; i++)
        {
            GameObject goToDestroy = juicinessParent.transform.GetChild(i).gameObject;
            /*UnityEditor.EditorApplication.delayCall += () =>
            {
                DestroyImmediate(goToDestroy);
            };*/
        }
    }

    void Start()
    {
        SetValuesOnEnable();
        for (int i = 0; i < activeJuices.Length; i++)
        {
            activeJuices[i].juicinessSpawner.SaveResetValue();
        }
        StartCoroutine(WaitBeforeShowText());
    }

    IEnumerator WaitBeforeShowText()
    {
        yield return new WaitForSeconds(1);
        canShow = true;
    }

    void ChangeName()
    {
        for (int i = 0; i < activeJuices.Length; i++)
        {

            if (activeJuices[i].whatToUse != ActiveJuiceValues.HowToModify.Title)
            {
                activeJuices[i].effectToActive = (activeJuices[i].whatToUse).ToString().ToUpper() + " : " + activeJuices[i].whichEffect.ToString();
            }
            else
            {
                activeJuices[i].effectToActive = "TITLE : " + activeJuices[i].titleName;
            }
        }
    }

    void GenerateButtons()
    {
        eventSystem.firstSelectedGameObject = null;
        for (int i = 0; i < activeJuices.Length; i++)
        {
            Transform juicinessTransform = Instantiate(juicinessToSpawn).transform;
            TextMeshProUGUI juicinessText = juicinessTransform.GetComponentInChildren<TextMeshProUGUI>();
            juicinessText.text = activeJuices[i].whichEffect.ToString();
            juicinessText.transform.name = "Juice : " + activeJuices[i].effectToActive + "Txt";
            juicinessTransform.transform.parent = juicinessParent;

            juicinessTransform.transform.name = "Juciness" + activeJuices[i].effectToActive + activeJuices[i].whatToUse;

            switch (activeJuices[i].whatToUse)
            {
                case ActiveJuiceValues.HowToModify.Toggle:
                    juicinessTransform.GetComponent<JuicinessSpawner>().toggle.gameObject.SetActive(true);
                    if (eventSystem.firstSelectedGameObject == null)
                    {
                        eventSystem.firstSelectedGameObject = juicinessTransform.GetComponent<JuicinessSpawner>().toggle.gameObject;
                    }
                    break;
                case ActiveJuiceValues.HowToModify.Slider:
                    juicinessTransform.GetComponent<JuicinessSpawner>().slider.gameObject.SetActive(true);
                    if (eventSystem.firstSelectedGameObject == null)
                    {
                        eventSystem.firstSelectedGameObject = juicinessTransform.GetComponent<JuicinessSpawner>().slider.gameObject;
                    }
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

            activeJuices[i].juicinessSpawner.keyPress = (KeyCode)System.Enum.Parse(typeof(KeyCode), "Keypad" + activeJuices[i].numpadToActive);

            if (activeJuices[i].whatToUse == ActiveJuiceValues.HowToModify.Title)
            {
                juicinessText.text = activeJuices[i].titleName;
                activeJuices[i].juicinessSpawner.name = "Title : " + activeJuices[i].titleName;
                juicinessText.fontSize = 60;
                juicinessText.fontStyle = FontStyles.Bold;
                juicinessTransform.GetComponentInChildren<Button>().gameObject.SetActive(false);
            }  

        }


        float sizeY = juicinessParent.transform.GetComponent<GridLayoutGroup>().cellSize.y * juicinessParent.transform.childCount;
        juicinessParent.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, sizeY / 2);
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

    public void SpawnText(string power, bool isEnable)
    {
        SetText(power, isEnable, Mathf.Infinity);
    }

    public void SpawnText(string power, float value)
    {
        SetText(power, true, value);
    }

    void SetText(string power, bool isEnable, float value)
    {
        if (!canShow)
            return;
        GameObject inst = Instantiate(juicinessTextInstance, juicinessTextParent);
        inst.transform.parent = juicinessTextParent;
        inst.transform.position = Vector3.zero;

        TextMeshProUGUI text = inst.GetComponentInChildren<TextMeshProUGUI>();

        if(value == Mathf.Infinity)
        {
            text.color = isEnable ? Color.green : Color.red;
            string enable = isEnable ? "Enable " : "Disable";
            text.text = power + " : " + enable;
            Destroy(inst, 2f);
        
        }
        else
        {
            text.color = value != 0 ? Color.green : Color.red;
            text.text = power + " : " + value;
            Destroy(inst, 2f);
        }
    }
}

