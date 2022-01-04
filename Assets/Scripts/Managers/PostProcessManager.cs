using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoBehaviour
{

    [SerializeField] Volume v;
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
        v.profile.TryGet(out b);
        v.profile.TryGet(out vg);
        v.profile.TryGet(out lsd);
        v.profile.TryGet(out ca);
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
}
