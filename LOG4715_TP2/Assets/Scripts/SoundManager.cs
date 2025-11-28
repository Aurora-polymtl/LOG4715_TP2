using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource sourceSound;
    private AudioSource sourceMusic;

    private void Awake()
    {
        sourceSound = GetComponent<AudioSource>();
        sourceMusic = transform.GetChild(0).GetComponent<AudioSource>();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        EditMusic(0);
        StopSound(0);
    }
    public void PlaySound(AudioClip sound)
    {
        sourceSound.PlayOneShot(sound);
    }

    public void StopSound(float sound)
    {
        ChangeVolume(1, "soundVolume", sound, sourceSound);
    }

    public void EditMusic(float music)
    {
        ChangeVolume(1, "musicVolume", music, sourceMusic);
    }

    private void ChangeVolume(float basicVolume, string volumeName, float volume, AudioSource audioSource)
    {
        float currentVolume = PlayerPrefs.GetFloat(volumeName, 1);
        currentVolume += volume;
        if (currentVolume > 1)
        {
            currentVolume = 0;

        }
        else if (currentVolume < 0)
        {
            currentVolume = 1;
        }
        float trueVolume = currentVolume *= basicVolume;
        audioSource.volume = trueVolume;
        PlayerPrefs.SetFloat(volumeName, currentVolume);
    }
}