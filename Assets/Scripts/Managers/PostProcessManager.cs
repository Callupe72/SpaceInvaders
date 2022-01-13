using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoBehaviour
{

    [SerializeField] Volume volume;
    Bloom b;
    Vignette vg;
    LensDistortion lsd;
    ChromaticAberration ca;
    ColorAdjustments colorA;

    [SerializeField] float bloomValue = .7f;

    //Color adjustment
    public enum PlayerState
    {
        isWaiting,
        isGoingRed,
        isGoingWhite,
    }

    Color currentColor;
    float currentTime;
    PlayerState playerState;

    Player player;


    public static PostProcessManager Instance;
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
    void Start()
    {
        volume.profile.TryGet(out b);
        volume.profile.TryGet(out vg);
        volume.profile.TryGet(out lsd);
        volume.profile.TryGet(out ca);
        volume.profile.TryGet(out colorA);
    }

    public void SetLensDistorsion(bool isActive, float intensity/*, float xMultiplier, float yMultiplier, Vector2 center, float scale*/)
    {
        lsd.active = isActive;
        if (isActive)
        {
            if (intensity != Mathf.Infinity)
                lsd.intensity.value = intensity;

            //if (xMultiplier != Mathf.Infinity)
            //    lsd.xMultiplier.value = xMultiplier;

            //if (yMultiplier != Mathf.Infinity)
            //    lsd.yMultiplier.value = yMultiplier;

            //if (center.x != Mathf.Infinity)
            //    lsd.center.value = center;

            //if (scale != Mathf.Infinity)
            //    lsd.scale.value = scale;
        }
    }

    public void SetChromaticAberration(bool isActive, float intensity)
    {
        ca.active = isActive;
        if (isActive)
        {
            ca.intensity.value = intensity;
        }
    }
    public void SetBloom(bool isActive, float intensity)
    {
        b.active = isActive;
        if (isActive)
        {
            b.intensity.value = intensity * ActiveJuiceManager.Instance.GetValueFloat(ActiveJuiceManager.ActiveJuiceValues.AllEffect.Bloom) * bloomValue;
        }
    }

    public float GetBloom()
    {
        return b.intensity.value;
    }

    public bool GetPostProcessIsActive()
    {
        return volume.enabled;
    }
    public void SetPostProcessIsActive(bool isActive)
    {
        volume.enabled = isActive;
    }

    void Update()
    {
        //COLOR ADJUSTMENT
        switch (playerState)
        {
            case PlayerState.isWaiting:
                break;
            case PlayerState.isGoingRed:
                currentTime += Time.deltaTime;
                colorA.colorFilter.value = currentColor;
                currentColor = Color.Lerp(Color.white, Color.red, currentTime);
                if (currentTime > 0.1f)
                {
                    currentTime = -1;
                    playerState = PlayerState.isGoingWhite;
                }
                break;
            case PlayerState.isGoingWhite:
                currentTime += Time.deltaTime;
                colorA.colorFilter.value = currentColor;
                currentColor = Color.Lerp(Color.red, Color.white, currentTime);
                if (currentTime > player.invisibilityTime)
                {
                    playerState = PlayerState.isWaiting;
                    player.isInvisibility = false;
                    currentTime = 0;
                }
                break;
            default:
                break;
        }
    }

    public void PlayerIsTouch(Player playerDamage)
    {
        if (!player)
        {
            player = playerDamage;
        }

        playerState = PlayerState.isGoingRed;
        player.isInvisibility = true;
        currentTime = 0;
    }

    public void DashChangeAberation()
    {
        DOTween.To(() => ca.intensity.value, x => ca.intensity.value = x, 0, .01f);
        DOTween.To(() => lsd.intensity.value, x => lsd.intensity.value = x, 0, .01f);
        DOTween.To(() => 10, x => ca.intensity.value = x, 0, .5f);
        DOTween.To(() => .25f, x => lsd.intensity.value = x, 0, .5f);
    }
}
