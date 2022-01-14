using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance = null;

    public Sound[] sounds;
    public Slider audioVolumeSlider;
    public TextMeshProUGUI audioValueText;

    bool canPlaySound = true;

    public float audioVolume { get; set; } = 50f;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            //Destroy(gameObject);
        }
    }
    void Start()
    {
        LoadAudioValue();
    }

    void LoadAudioValue()
    {
        if (PlayerPrefs.GetFloat("audioVolume") > 0)
            audioVolumeSlider.value = PlayerPrefs.GetFloat("audioVolume");
        else
            audioVolumeSlider.value = 50f;

        ActualiseText();
    }
    public void SaveAudioValue()
    {
        PlayerPrefs.SetFloat("audioVolume", audioVolume);
        ActualiseText();
    }

    public void SetAudioValueFromSlider()
    {
        audioVolume = audioVolumeSlider.value;
        ActualiseText();
    }

    public void ActualiseText()
    {
        audioValueText.text = audioVolume.ToString();
    }

    public void Play2DSound(string name)
    {
        if (!canPlaySound)
            return;
        Sound s = Array.Find(sounds, sound => sound.name == name);

        GameObject obj2D = new GameObject();
        obj2D.AddComponent<AudioSource>();
        AudioSource source2D = obj2D.GetComponent<AudioSource>();

        obj2D.name = name;

        source2D.clip = s.clip;
        source2D.loop = s.canLoop;
        source2D.volume = s.volume * (audioVolume / 100 * (Mathf.Clamp(AudioReaction.Instance.GetDropValue(),.5f,5)));
        source2D.pitch = s.pitch;
        source2D.PlayOneShot(source2D.clip);


        if (!s.canLoop)
            Destroy(obj2D, source2D.clip.length);
    }

    public void Play3DSound(string name, Vector3 positionToPlay)
    {
        if (!canPlaySound)
            return;

        Sound s = Array.Find(sounds, sound => sound.name == name);

        GameObject obj3D = new GameObject();
        obj3D.transform.position = positionToPlay;
        obj3D.AddComponent<AudioSource>();
        AudioSource source3D = obj3D.GetComponent<AudioSource>();

        obj3D.name = name;

        source3D.clip = s.clip;
        source3D.minDistance = s.minDist;
        source3D.maxDistance = s.maxDist;
        source3D.volume = s.volume * (audioVolume/100);
        source3D.loop = s.canLoop;
        source3D.pitch = s.pitch;
        source3D.spatialBlend = 1;
        source3D.rolloffMode = AudioRolloffMode.Linear;

        if (!source3D.loop)
        {
            Destroy(obj3D, source3D.clip.length);
            source3D.PlayOneShot(source3D.clip);
        }
        else
        {
            source3D.Play();
        }
    }
    public void StopSound(string name)
    {
        foreach (AudioSource audio in FindObjectsOfType<AudioSource>())
        {
            if (audio.name == name)
            {
                Destroy(audio.gameObject);
            }
        }
    }

    public void SetCanPlaySound(bool canPlay)
    {
        canPlaySound = canPlay;
    }

    public bool GetCanPlaySound()
    {
        return canPlaySound;
    }
}
