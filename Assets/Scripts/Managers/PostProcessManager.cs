using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoBehaviour
{

    [SerializeField] Volume volume;
    Bloom b;
    Vignette vg;
    LensDistortion lsd;
    ChromaticAberration ca;

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
            b.intensity.value = intensity * ActiveJuiceManager.Instance.GetValueFloat(ActiveJuiceManager.ActiveJuiceValues.AllEffect.Bloom);
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
}
