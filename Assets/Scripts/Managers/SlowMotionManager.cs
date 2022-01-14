using UnityEngine;

public class SlowMotionManager : MonoBehaviour
{
    public static SlowMotionManager Instance;
    float slowDownLenght = 2f;
    [SerializeField] AnimationCurve slowMoCurve;
    bool slowMo;
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

        if (!slowMo)
            return;
        //slowMoCurrentTime += Time.unscaledDeltaTime;
        Time.timeScale += (1f / slowDownLenght) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0, 1);
        if (Time.timeScale >= 1)
            slowMo = false;
    }

    public void SlowMotion(float slowDownFactor)
    {

        if (!ActiveJuiceManager.Instance.SlowmotionIsOn)
            return;

        slowMo = true;
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void SlowMotion()
    {
        SlowMotion(0.05f);
    }
}
