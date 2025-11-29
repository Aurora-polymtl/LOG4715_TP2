using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource source;
    [SerializeField] private GameObject musicObject;
    private AudioSource musicSource;

    public float MusicVolume { get; private set; }
    public float SoundVolume { get; private set; }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            musicSource = musicObject.GetComponent<AudioSource>();
            musicSource.loop = true;
            MusicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
            SoundVolume = PlayerPrefs.GetFloat("soundVolume", 1f);

            musicSource.volume = MusicVolume;
            source.volume = SoundVolume;
        }
        else if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip music)
    {
        if (music != musicSource.clip)
        {
            musicSource.clip = music;
            musicSource.Play();
        }
    }

    public void PlaySound(AudioClip sound)
    {
        source.PlayOneShot(sound);
    }

    public void SetMusicVolume(float value)
    {
        MusicVolume = value;
        musicSource.volume = value;
        PlayerPrefs.SetFloat("musicVolume", value);
    }

    public void SetSoundVolume(float value)
    {
        SoundVolume = value;
        source.volume = value;
        PlayerPrefs.SetFloat("soundVolume", value);
    }
}