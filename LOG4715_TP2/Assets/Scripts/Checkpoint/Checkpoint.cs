using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public BoxCollider2D trigger;
    public SpriteRenderer flagRenderer;
    public Sprite activatedSprite;
    public ParticleSystem flagParticles;
    [SerializeField] private AudioClip checkpointMarkSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            RespawnController.instance.respawnPoint = transform;
            trigger.enabled = false;
            flagRenderer.sprite = activatedSprite;
            flagParticles.Play();
            SoundManager.instance.PlaySound(checkpointMarkSound);
        }
    }
}
