using UnityEngine;
using UnityEngine.SceneManagement; // nécessaire pour changer de scène

public class MenuManager : MonoBehaviour
{
    // Nom de la scène de jeu à mettre dans l'inspecteur
    [SerializeField] private string gameSceneName = "Level 1";
    public GameObject mainMenuScreen;

    // Appelé par le bouton Jouer
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Appelé par le bouton Quitter
    public void QuitGame()
    {
        Debug.Log("Quitter le jeu !"); // utile pour tester dans l'éditeur
        Application.Quit();
    }

    public void ReturnMainMenu(GameObject currentScreen)
    {
        currentScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }

    public void OpenScreen(GameObject screenToOpen)
    {
        mainMenuScreen.SetActive(false);
        screenToOpen.SetActive(true);
    }
}
