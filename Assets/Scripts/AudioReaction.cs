using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] Slider volumeSlider;
    [SerializeField] TextMeshProUGUI volumeTxt;

    float dropValue = 100;
    [SerializeField] float soundImpact = 100;

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

    void Start()
    {
        SetVolume();
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
                //if (clipLoudness > 200)
                //{
                //    dropValue = clipLoudness;
                //}
                //else
                //{
                //    dropValue = 100;
                //}

                dropValue = clipLoudness;

                if (!SceneManager.GetActiveScene().name.Contains("Menu"))
                    PostProcessManager.Instance.SetBloom(true, clipLoudness * soundImpact / 1000);
            }
        }

    }

    public void SetVolume()
    {
        if (SceneManager.GetActiveScene().name.Contains("Menu"))
            return;
        if (audioSource == null)
            GameObject.Find("MusicSlider").GetComponent<Slider>();
        audioSource.volume = (float)volumeSlider.value / 100;
        volumeTxt.text = volumeSlider.value.ToString();
    }

    public float GetDropValue()
    {
        return Mathf.Clamp(dropValue / 100, 1, 100);
    }

    public void PauseMusic()
    {
        audioSource.Pause();
    }

    public void PlayMusic()
    {
        audioSource.Play();
    }
}
