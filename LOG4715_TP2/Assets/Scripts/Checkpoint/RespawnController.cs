using UnityEngine;

public class RespawnController : MonoBehaviour
{
    public static RespawnController instance;
    public Transform respawnPoint;

    void Awake()
    {
        instance = this;
    }

    public void RespawnPlayer()
    {
        var player = GameObject.FindGameObjectWithTag("Player")
                  ?? Object.FindFirstObjectByType<PlayerMove2D>(FindObjectsInactive.Exclude)?.gameObject;

        if (player == null || respawnPoint == null)
        {
            return;
        }
        RespawnPlayer(player);
    }

    public void RespawnPlayer(GameObject player)
    {
        if (player == null || respawnPoint == null) return;

        var safe = player.GetComponent<PlayerSafeGround>();
        if (safe != null)
        {
            Vector2 target = respawnPoint.position;
            safe.ForceSetLastSafe(target);
            safe.TeleportTo(target, ignoreHazard: false);
            return;
        }

        // Fallback si pas de PlayerSafeGround
        var rb = player.GetComponent<Rigidbody2D>();
        var col = player.GetComponent<Collider2D>();
        bool prev = col && col.enabled;
        if (col) col.enabled = false;

        if (rb) { rb.linearVelocity = Vector2.zero; rb.position = respawnPoint.position; }
        else { player.transform.position = respawnPoint.position; }

        if (col) col.enabled = prev;
    }
}
