using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    [SerializeField] private TextMeshProUGUI musicValueText;
    [SerializeField] private TextMeshProUGUI soundValueText;

    private void Start()
    {
        // Initialiser les valeurs
        float musicValue = PlayerPrefs.GetFloat("musicVolume", 1f);
        float soundValue = PlayerPrefs.GetFloat("soundVolume", 1f);

        musicSlider.value = musicValue;
        soundSlider.value = soundValue;

        UpdateMusicText(musicValue);
        UpdateSoundText(soundValue);

        // Écouter les modifications
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        soundSlider.onValueChanged.AddListener(OnSoundSliderChanged);
    }

    private void OnMusicSliderChanged(float value)
    {
        SoundManager.instance.SetMusicVolume(value);
        UpdateMusicText(value);
    }

    private void OnSoundSliderChanged(float value)
    {
        SoundManager.instance.SetSoundVolume(value);
        UpdateSoundText(value);
    }

    private void UpdateMusicText(float value)
    {
        musicValueText.text = Mathf.RoundToInt(value * 100).ToString();
    }

    private void UpdateSoundText(float value)
    {
        soundValueText.text = Mathf.RoundToInt(value * 100).ToString();
    }
}
