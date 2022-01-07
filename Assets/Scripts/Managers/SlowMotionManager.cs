using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionManager : MonoBehaviour
{
    public static SlowMotionManager Instance;
    float newTime;
    float slowMoCurrentTime;
    bool canGoDown;
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
        if (canGoDown)
        {
            slowMoCurrentTime += Time.deltaTime;
            Time.timeScale += (1f / slowMoCurve.Evaluate(slowMoCurrentTime)) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0, 1);
            if (slowMoCurrentTime > newTime)
                canGoDown = false;
        }
    }

    public void SlowMotion(float slowMotionFactor, float slowMotionTime)
    {
        Time.timeScale = slowMotionFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        newTime = slowMotionTime;
        canGoDown = true;
    }

    public void SlowMotion(float slowMotionTime)
    {
        SlowMotion(0.05f, slowMotionTime);
    }
}
