using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UnlockNewPowerManager : MonoBehaviour
{
    [SerializeField] GameObject canvaToActive;
    [SerializeField] TextMeshProUGUI powerTitleText;
    [SerializeField] TextMeshProUGUI powerDescriptionText;
    [SerializeField] Image powerImage;
    [SerializeField] Image cooldown;

    [SerializeField] Player player;

    [Header("See new power")]
    public bool waitSeeNewPower;
    [SerializeField] float timePlayerValidNewPower = 1;
    float timeWait;

    public enum AttackId
    {
        ChargedAttack,
        Dash,
        DashMakesDamages,
        DestroyLine,
        Aim,
        DebriesCanMakesDamages,
    }
    [System.Serializable]
    public struct PowerStruct
    {
        public string powerName;
        public Sprite powerImg;
        [TextArea(5,1)] public string powerDescription;
        public UnlockNewPowerManager.AttackId attackId;
    }


    [SerializeField] PowerStruct[] powers;

    [SerializeField] int powerToUnlockIndex = -1;

    public static UnlockNewPowerManager Instance;
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

    public void AddNewPower()
    {
        waitSeeNewPower = true;
        powerToUnlockIndex++;
        SetCanvaActive(true);
        powerTitleText.text = powers[powerToUnlockIndex].powerName;
        powerDescriptionText.text = powers[powerToUnlockIndex].powerDescription;
        powerImage.sprite = powers[powerToUnlockIndex].powerImg;
        timeWait = 0;
        SwitchActivePowers();
    }

    void EndSeeNewPower()
    {
        waitSeeNewPower = false;
        timeWait = 0;
        SetCanvaActive(false);
        EnemySpawnerManager.Instance.CreateNewWave();
    }

    void Update()
    {
        if (waitSeeNewPower)
        {
            if (Input.GetButton("Fire"))
            {
                timeWait += Time.deltaTime;
                cooldown.fillAmount = (timeWait / timePlayerValidNewPower);
                cooldown.transform.localScale = Vector2.one * (timeWait / timePlayerValidNewPower) * 2;
                if (timeWait > timePlayerValidNewPower)
                {
                    EndSeeNewPower();
                }
            }

            else if (Input.GetButtonUp("Fire"))
            {
                timeWait = 0;
                cooldown.fillAmount = 0;
                cooldown.transform.localScale = Vector2.zero;
            }
        }
    }


    void SwitchActivePowers()
    {
        switch (powers[powerToUnlockIndex].attackId)
        {
            case AttackId.ChargedAttack:
                player.playerCanUsePowerShot = true;
                break;
            case AttackId.Dash:
                player.playerCanDash = true;
                break;
            case AttackId.DashMakesDamages:
                player.playerCanMakeDamagesOnDash = true;
                break;
            case AttackId.DestroyLine:
                player.playerCanDestroyLine = true;
                break;
            case AttackId.Aim:
                player.playerCanAim = true;
                break;
            case AttackId.DebriesCanMakesDamages:
                player.debrisMakesDamages = true;
                break;
            default:
                break;
        }
    }

    public void SetCanvaActive(bool active)
    {
        canvaToActive.SetActive(active);
    }
}
