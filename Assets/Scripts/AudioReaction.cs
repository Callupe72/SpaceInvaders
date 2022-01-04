using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReaction : MonoBehaviour
{
    [Space(20)]
    public AudioSource audioSource;
    public float updateStep = 0.1f;
    public int sampleDataLength = 1024;

    private float currentUpdateTime = 0f;

    private float clipLoudness;
    private float[] clipSampleData;
    public static AudioReaction Instance;

    float dropValue = 100;

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
        clipSampleData = new float[sampleDataLength];
    }
    void Update()
    {
        currentUpdateTime += Time.deltaTime;
        if (currentUpdateTime >= updateStep)
        {
            currentUpdateTime = 0f;
            audioSource.clip.GetData(clipSampleData, audioSource.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
            clipLoudness = 0f;
            foreach (var sample in clipSampleData)
            {
                clipLoudness += Mathf.Abs(sample);
                if (clipLoudness > 200)
                {
                    dropValue = clipLoudness;
                }
                else
                {
                    dropValue = 100;
                }

               PostProcessManager.Instance.SetBloom(true,clipLoudness/100);
            }
        }

    }

    public int GetDropValue()
    {
        return Mathf.RoundToInt(dropValue) / 100;
    }
}
