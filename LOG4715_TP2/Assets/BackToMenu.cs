using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Name of the menu scene to load when the player touches this trigger")]
    private string menuSceneName = "MenuScene";
    [SerializeField] private AudioClip victorySound;


    private void OnTriggerEnter2D(Collider2D other)
    {
        // only react to the player touching the trigger
        if (!other.CompareTag("Player")) return;
        SoundManager.instance.PlaySound(victorySound);

        // Load the menu scene
        if (!string.IsNullOrEmpty(menuSceneName))
        {
            SceneManager.LoadScene(menuSceneName);
        }
    }
}
