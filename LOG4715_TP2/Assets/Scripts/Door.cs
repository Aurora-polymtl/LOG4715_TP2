using UnityEngine;

public class door : MonoBehaviour
{
    private Collider2D doorCollider;
    private Rigidbody2D doorBody;
    private SpriteRenderer doorRenderer;
    [SerializeField] private AudioClip doorSound;

    void Awake()
    {
        // Récupère les composants du parent
        doorCollider = GetComponent<Collider2D>();
        doorBody = GetComponent<Rigidbody2D>();

        // Récupère le SpriteRenderer dans l'objet enfant
        doorRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void OpenDoor()
    {
        // Désactive les collisions et la physique
        if (doorCollider != null)
            doorCollider.enabled = false;

        if (doorBody != null)
            doorBody.simulated = false;

        // Rend la porte invisible
        if (doorRenderer != null)
            doorRenderer.enabled = false;
            SoundManager.instance.PlaySound(doorSound);
    }

    public void CloseDoor()
    {
        if (doorCollider != null)
            doorCollider.enabled = true;

        if (doorBody != null)
            doorBody.simulated = true;

        if (doorRenderer != null)
            doorRenderer.enabled = true;

    }
}
