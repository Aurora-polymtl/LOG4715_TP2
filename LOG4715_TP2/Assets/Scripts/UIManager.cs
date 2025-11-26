using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject endScreen;


    private void Awake()
    {
        pauseScreen.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {
            if (pauseScreen.activeInHierarchy)
            {
                Pause(false);
            }
            else
            {
                Pause(true);
            }
        }
    }
    public void Pause(bool status)
    {
        pauseScreen.SetActive(status);
        if (status)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void SoundVolume() { }

    public void MusicVolume() { }

}
