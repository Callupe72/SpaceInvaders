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

    public KeyCode keyPress;

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

    void Update()
    {
        if (Input.GetKeyDown(keyPress))
        {
            if (toggle.gameObject.activeInHierarchy)
            {
                toggle.isOn = !toggle.isOn;
            }
        }
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
                ActiveJuiceManager.Instance.ExplosionIsOn = toggle.isOn;
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
                ActiveJuiceManager.Instance.AnimationIsOn = toggle.isOn;
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.XpBar:
                XPManager.Instance.SetXpBar(toggle.isOn);
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.XpText:
                XPManager.Instance.SetXpText(toggle.isOn);
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Particles:
                ParticlesManager.Instance.SetCanParticles(toggle.isOn);
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Combo:
                ComboManager.Instance.SetCanCombo(toggle.isOn);
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.BarrelRoll:
                ActiveJuiceManager.Instance.BarrelRollIsOn = toggle.isOn;
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.SlowMotion:
                ActiveJuiceManager.Instance.SlowmotionIsOn = toggle.isOn;
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Zoom:
                ActiveJuiceManager.Instance.ZoomIsOn = toggle.isOn;
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Trails:
                ActiveJuiceManager.Instance.TrailsIsOn = toggle.isOn;
                TrailRenderer[] allTrail =  FindObjectsOfType<TrailRenderer>();
                foreach (TrailRenderer item in allTrail)
                {
                    item.enabled = ActiveJuiceManager.Instance.TrailsIsOn;
                }
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.PerfectText:
                ActiveJuiceManager.Instance.PerfectTextIsOn = toggle.isOn;
                break;
            default:
                break;
        }

        if (toggle.gameObject.activeInHierarchy)
        {
            ActiveJuiceManager.Instance.SpawnText(thisEffect.ToString(), toggle.isOn);
        }
        else
        {
            ActiveJuiceManager.Instance.SpawnText(thisEffect.ToString(), slider.value);
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
                toggle.SetIsOnWithoutNotify(PostProcessManager.Instance.GetPostProcessIsActive());
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Bloom:
                slider.SetValueWithoutNotify(PostProcessManager.Instance.GetBloom());
                sliderText.text = slider.value.ToString();
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.ShipExplostion:
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Sound:
                toggle.SetIsOnWithoutNotify(AudioManager.Instance.GetCanPlaySound());
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Music:
                toggle.SetIsOnWithoutNotify(AudioReaction.Instance.audioSource.isPlaying);
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.ShakeCamera:
                toggle.SetIsOnWithoutNotify(CinemachineShake.Instance.GetCanShake());
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Animations:
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.XpText:
                toggle.SetIsOnWithoutNotify(XPManager.Instance.GetXpText());
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.XpBar:
                toggle.SetIsOnWithoutNotify(XPManager.Instance.GetXpBar());
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Particles:
                toggle.SetIsOnWithoutNotify(ParticlesManager.Instance.GetCanParticles());
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Combo:
                toggle.SetIsOnWithoutNotify(ComboManager.Instance.GetCanCombo());
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.BarrelRoll:
                toggle.SetIsOnWithoutNotify(ActiveJuiceManager.Instance.BarrelRollIsOn);
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.SlowMotion:
                toggle.SetIsOnWithoutNotify(ActiveJuiceManager.Instance.SlowmotionIsOn);
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Zoom:
                toggle.SetIsOnWithoutNotify(ActiveJuiceManager.Instance.ZoomIsOn);
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Trails:
                toggle.SetIsOnWithoutNotify(ActiveJuiceManager.Instance.TrailsIsOn);
                TrailRenderer[] allTrail = FindObjectsOfType<TrailRenderer>();
                foreach (TrailRenderer item in allTrail)
                {
                    item.enabled = ActiveJuiceManager.Instance.TrailsIsOn;
                }
                break;
            default:
                break;
        }
    }

    public void ResetToDefaultValue()
    {
        if (thisEffect == ActiveJuiceManager.ActiveJuiceValues.AllEffect.ActiveEveryhing)
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
            ActiveJuiceManager.Instance.SpawnText(thisEffect.ToString(), slider);
        }
        else
        {
            toggle.isOn = defaultValueBool;
            ActiveJuiceManager.Instance.SpawnText(thisEffect.ToString(), defaultValueBool);
        }

    }

    public void PlaySound(string soundToPlay)
    {
        AudioManager.Instance.Play2DSound(soundToPlay);
    }
}