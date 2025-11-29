using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Name of the menu scene to load when the player touches this trigger")]
    private string menuSceneName = "MenuScene";

    private void OnTriggerEnter2D(Collider2D other)
    {
        // only react to the player touching the trigger
        if (!other.CompareTag("Player")) return;

        // optional: ensure this collider is a trigger - usually set in the editor
        if (!GetComponent<Collider2D>())
        {
            Debug.LogWarning("BackToMenu requires a Collider2D (set IsTrigger = true) on the same GameObject.");
        }

        // Load the menu scene
        if (!string.IsNullOrEmpty(menuSceneName))
        {
            SceneManager.LoadScene(menuSceneName);
        }
        else
        {
            Debug.LogWarning("BackToMenu: menuSceneName is empty. Set the scene name in the inspector.");
        }
    }
}
