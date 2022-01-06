using System.Collections;
using System.Collections.Generic;
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
            PostProcess,
            Bloom,
            ShipExplostion,
            Sound,
            Music,
            ShakeCamera,
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
            foreach (Transform item in juicinessParent.transform)
            {
                Destroy(item.gameObject);
            }
        }

        ChangeName();
    }

    void Start()
    {
        SetValuesOnEnable();
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

            if(activeJuices[i].whatToUse == ActiveJuiceValues.HowToModify.Title)
            {
                juicinessText.text = "Title : " +  activeJuices[i].titleName;
                juicinessText.fontSize = 30;
            }

        }
    }

    public float GetValueFloat(ActiveJuiceValues.AllEffect whichEffect)
    {
        for (int i = 0; i < activeJuices.Length; i++)
        {
            if(activeJuices[i].whichEffect == whichEffect)
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
}
