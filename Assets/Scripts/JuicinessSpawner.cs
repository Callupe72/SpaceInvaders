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
    public ActiveJuiceManager.ActiveJuiceValues.AllEffect thisEffect;

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

        switch (thisEffect)
        {
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

}
