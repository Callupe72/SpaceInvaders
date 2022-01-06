using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    CinemachineVirtualCamera cinemachineVirtualCamera;
    float shakerTime;

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

    public void ShakeCamera(float intensity, float time)
    {
        if (!canShake)
            return;

        CinemachineBasicMultiChannelPerlin cinemachineMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakerTime = time;
    }

    void Update()
    {
        if (shakerTime > 0)
        {
            shakerTime -= Time.deltaTime;
            if (shakerTime <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
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
