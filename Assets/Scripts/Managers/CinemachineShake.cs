using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    CinemachineVirtualCamera cinemachineVirtualCamera;
    float shakerTime;
    float shakerTimeTotal;
    float startingIntensity;


    [SerializeField] bool canShake = true;

    public static CinemachineShake Instance;

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
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();

    }

   void Start()
    {
        ShakeCamera(0, 0);
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakerTime = time;
        startingIntensity = intensity;
        shakerTimeTotal = intensity;
    }

    void Update()
    {
        if (shakerTime > 0)
        {
            shakerTime -= Time.deltaTime;
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity,0f, 1 -(shakerTime / shakerTimeTotal));
            
        }
    }

    public bool GetCanShake()
    {
        return canShake;
    }

    public void SetCanShake(bool canI)
    {
        canShake = canI;
    }
}
