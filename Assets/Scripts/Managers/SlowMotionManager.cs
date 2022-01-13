using UnityEngine;

public class SlowMotionManager : MonoBehaviour
{
    public static SlowMotionManager Instance;
    float slowMoCurrentTime;
    [SerializeField] AnimationCurve slowMoCurve;

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

    void Update()
    {
        slowMoCurrentTime += Time.unscaledDeltaTime;
        Time.timeScale += (1f / slowMoCurrentTime) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0, 1);
    }

    public void SlowMotion(float slowMotionFactor)
    {

        if (!ActiveJuiceManager.Instance.SlowmotionIsOn)
            return;

        Time.timeScale = slowMotionFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        slowMoCurrentTime = 0;
    }

    public void SlowMotion()
    {
        SlowMotion(0.05f);
    }
}
