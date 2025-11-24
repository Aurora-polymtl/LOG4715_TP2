using UnityEngine;
using UnityEngine.SceneManagement; // nécessaire pour changer de scène

public class MenuManager : MonoBehaviour
{
    // Nom de la scène de jeu à mettre dans l'inspecteur
    [SerializeField] private string gameSceneName = "Level 1";

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
}
