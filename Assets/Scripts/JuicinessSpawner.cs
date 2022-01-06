using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JuicinessSpawner : MonoBehaviour
{
    public Toggle toggle;
    [SerializeField] TextMeshProUGUI sliderText;
    public Slider slider;

    public ActiveJuiceManager.ActiveJuiceValues.HowToModify modifyValue;
    public ActiveJuiceManager.ActiveJuiceValues.AllEffect thisEffect;

    float defaultValueFloat;
    bool defaultValueBool;

    public void SaveResetValue()
    {
        if (slider.gameObject.activeInHierarchy)
        {
            defaultValueFloat = slider.value;
        }
        else
        {
            defaultValueBool = toggle.isOn;
        }
    }

    public void SetText()
    {
        sliderText.text = slider.value.ToString();
    }

    public void ChangeValue()
    {
        switch (thisEffect)
        {
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.ActiveEveryhing:
                ActiveJuiceManager.Instance.ActiveAllToggle(toggle.isOn);
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.PostProcess:
                PostProcessManager.Instance.SetPostProcessIsActive(toggle.isOn);
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Bloom:
                PostProcessManager.Instance.SetBloom(true, slider.value);
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.ShipExplostion:
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Sound:
                AudioManager.Instance.SetCanPlaySound(toggle.isOn);
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Music:
                if (toggle.isOn) { AudioReaction.Instance.PlayMusic(); } else { AudioReaction.Instance.PauseMusic(); }
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.ShakeCamera:
                CinemachineShake.Instance.SetCanShake(toggle.isOn);
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Animations:
                break;
            default:
                break;
        }

        if (thisEffect != ActiveJuiceManager.ActiveJuiceValues.AllEffect.ActiveEveryhing)
        {
            if (toggle.gameObject.activeInHierarchy)
            {
                ActiveJuiceManager.Instance.ChangeActiveAllToggle(toggle.isOn);
            }
        }

    }

    public void SetButtonTo()
    {
        switch (thisEffect)
        {
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.PostProcess:
                toggle.isOn = PostProcessManager.Instance.GetPostProcessIsActive();
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Bloom:
                slider.value = PostProcessManager.Instance.GetBloom() * 10;
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.ShipExplostion:
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Sound:
                toggle.isOn = AudioManager.Instance.GetCanPlaySound();
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Music:
                toggle.isOn = AudioReaction.Instance.audioSource.isPlaying;
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.ShakeCamera:
                toggle.isOn = CinemachineShake.Instance.GetCanShake();
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Animations:
                break;
            default:
                break;
        }
    }

    public void ResetToDefaultValue()
    {
        if(thisEffect == ActiveJuiceManager.ActiveJuiceValues.AllEffect.ActiveEveryhing)
        {
            ActiveJuiceManager.Instance.ResetAllButtons();
        }
        ResetButtons();
    }

    public void ResetButtons()
    {
        if (slider.gameObject.activeInHierarchy)
        {
            slider.value = defaultValueFloat;
        }
        else
        {
            toggle.isOn = defaultValueBool;
        }
    }
}
