using UnityEngine;
using UnityEngine.UI;
public class VolumeText : MonoBehaviour
{
    [SerializeField] private string volumeNameText;
    [SerializeField] private string intro;
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }
    private void Update()
    {
        UpdateVolume();
    }
    private void UpdateVolume()
    {
        float volumeValue = PlayerPrefs.GetFloat(volumeNameText) * 100;

        text.text = intro + volumeValue.ToString();
    }
}
