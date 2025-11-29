using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (RespawnController.instance != null)
        {
            var hp = collision.GetComponent<Health>();
            hp.TakingDamage(1f, hitSide: 0, applyKnockback: false);
            RespawnController.instance.RespawnPlayer(collision.gameObject);
        }
    }
}
