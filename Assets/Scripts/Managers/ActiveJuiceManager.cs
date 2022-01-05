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
            ShakeCamera,
            Animations,
        }

        [Header("Essentials")]
        public AllEffect whichEffect;
        public bool isActive;



        public enum HowToModify
        {
            Toggle,
            Slider,
        }

        public HowToModify whatToUse;

        [Header("Slider")]

        public float minValue;
        public float maxValue;
        public bool wholeNumbers;

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
            juicinessTransform.GetComponentInChildren<TextMeshProUGUI>().text = activeJuices[i].effectToActive;
            juicinessTransform.GetComponentInChildren<TextMeshProUGUI>().transform.name = "Juice : " + activeJuices[i].effectToActive + "Txt";
            juicinessTransform.transform.parent = juicinessParent;

            juicinessTransform.transform.name = "Juciness" + activeJuices[i].effectToActive;

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
            JuicinessSpawner juicinessSpawner = juicinessTransform.GetComponent<JuicinessSpawner>();

            juicinessSpawner.modifyValue = activeJuices[i].whatToUse;
            juicinessSpawner.slider.minValue = activeJuices[i].minValue;
            juicinessSpawner.slider.maxValue = activeJuices[i].maxValue;
            juicinessSpawner.slider.wholeNumbers = activeJuices[i].wholeNumbers;

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

