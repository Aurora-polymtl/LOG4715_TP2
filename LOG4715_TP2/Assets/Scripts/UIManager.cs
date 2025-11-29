using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private string mainMenuSceneName = "MenuScene";

    private void Awake()
    {
        pauseScreen.SetActive(false);
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionsPanel.activeInHierarchy)
            {
                CloseOptions();
            }
            else 
            { 
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

    public void OpenOptions()
    {
        pauseScreen.SetActive(false);   
        optionsPanel.SetActive(true);   
        //Time.timeScale = 0;            
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        pauseScreen.SetActive(true);   
        //Time.timeScale = 0;            
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }
}
