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
    }
    public void PlaySound(AudioClip sound)
    {
        sourceSound.PlayOneShot(sound);
    }

    public void StopSound(float sound)
    {
        float currentVolume = PlayerPrefs.GetFloat("soundVolume");
        currentVolume += sound;
        if(currentVolume > 1)
        {
            currentVolume = 0;

        }
        else if(currentVolume < 0)
        {
            currentVolume = 1;
        }
        sourceSound.volume = currentVolume;
        PlayerPrefs.SetFloat("soundVolume", currentVolume);
    }

    public void StopMusic(float sound)
    {
        float currentVolume = PlayerPrefs.GetFloat("musicVolume");
        currentVolume += sound;
        if (currentVolume > 1)
        {
            currentVolume = 0;

        }
        else if (currentVolume < 0)
        {
            currentVolume = 1;
        }

        sourceMusic.volume = currentVolume;
        PlayerPrefs.SetFloat("musicVolume", currentVolume);
    }
}