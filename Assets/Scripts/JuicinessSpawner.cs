using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class JuicinessSpawner : MonoBehaviour
{
    public Toggle toggle;
    [SerializeField] TextMeshProUGUI sliderText;
    public Slider slider;

    public ActiveJuiceManager.ActiveJuiceValues.HowToModify modifyValue;

    public void SetText()
    {
        sliderText.text = slider.value.ToString();
    }

    public void ChangeValue()
    {
        switch (modifyValue)
        {
            case ActiveJuiceManager.ActiveJuiceValues.HowToModify.Toggle:

                //Toggle true false

                break;
            case ActiveJuiceManager.ActiveJuiceValues.HowToModify.Slider:

                //Value en Int

                break;
            default:
                break;
        }
    }
}
