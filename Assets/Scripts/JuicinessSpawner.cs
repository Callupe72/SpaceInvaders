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
                TrailRenderer[] allTrail = FindObjectsOfType<TrailRenderer>();
                foreach (TrailRenderer item in allTrail)
                {
                    item.enabled = ActiveJuiceManager.Instance.TrailsIsOn;
                }
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.PerfectText:
                PerfectManager.Instance.canPerfect = toggle.isOn;
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.TextDamages:
                EnemySpawnerManager.Instance.canSpawnDamageText = toggle.isOn;
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.EnemyStillAlive:
                EnemySpawnerManager.Instance.canSpawnTextEnemyRestants = toggle.isOn;
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Score:
                ScoreManager.Instance.canScoreGrow = toggle.isOn;
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
                toggle.isOn = false;
                //toggle.SetIsOnWithoutNotify(PostProcessManager.Instance.GetPostProcessIsActive());
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Bloom:
                slider.value = 1f;
                //slider.SetValueWithoutNotify(PostProcessManager.Instance.GetBloom());
                sliderText.text = slider.value.ToString();
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.ShipExplostion:
                toggle.isOn = false;
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Sound:
                toggle.isOn = false;
                //toggle.SetIsOnWithoutNotify(AudioManager.Instance.GetCanPlaySound());
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Music:
                toggle.isOn = false;
                //toggle.SetIsOnWithoutNotify(AudioReaction.Instance.audioSource.isPlaying);
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.ShakeCamera:
                toggle.isOn = false;
                //toggle.SetIsOnWithoutNotify(CinemachineShake.Instance.GetCanShake());
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Animations:
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.XpText:
                toggle.isOn = false;
                //toggle.SetIsOnWithoutNotify(XPManager.Instance.GetXpText());
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.XpBar:
                toggle.isOn = false;
                //toggle.SetIsOnWithoutNotify(XPManager.Instance.GetXpBar());
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Particles:
                toggle.isOn = false;
                //toggle.SetIsOnWithoutNotify(ParticlesManager.Instance.GetCanParticles());
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Combo:
                toggle.isOn = false;
                //toggle.SetIsOnWithoutNotify(ComboManager.Instance.GetCanCombo());
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.BarrelRoll:
                toggle.isOn = false;
                //toggle.SetIsOnWithoutNotify(ActiveJuiceManager.Instance.BarrelRollIsOn);
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.SlowMotion:
                toggle.isOn = false;
                //toggle.SetIsOnWithoutNotify(ActiveJuiceManager.Instance.SlowmotionIsOn);
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Zoom:
                toggle.isOn = false;
                //toggle.SetIsOnWithoutNotify(ActiveJuiceManager.Instance.ZoomIsOn);
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Trails:
                toggle.isOn = false;
                //toggle.SetIsOnWithoutNotify(ActiveJuiceManager.Instance.TrailsIsOn);
                TrailRenderer[] allTrail = FindObjectsOfType<TrailRenderer>();
                foreach (TrailRenderer item in allTrail)
                {
                    //item.enabled = ActiveJuiceManager.Instance.TrailsIsOn;
                    item.enabled = false;
                }
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.ActiveEveryhing:
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.PerfectText:
                toggle.isOn = false;
                //toggle.SetIsOnWithoutNotify(PerfectManager.Instance.canPerfect);
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.TextDamages:
                toggle.isOn = false;
                //toggle.SetIsOnWithoutNotify(EnemySpawnerManager.Instance.canSpawnDamageText);
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.EnemyStillAlive:
                toggle.isOn = false;
                //toggle.SetIsOnWithoutNotify(EnemySpawnerManager.Instance.canSpawnTextEnemyRestants);
                break;
            case ActiveJuiceManager.ActiveJuiceValues.AllEffect.Score:
                toggle.isOn = false;
                //toggle.SetIsOnWithoutNotify(ScoreManager.Instance.canScoreGrow);
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