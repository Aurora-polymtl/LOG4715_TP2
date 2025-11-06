using UnityEngine;

public class SpikeHazard : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var hp = other.GetComponent<Health>();
        var safe = other.GetComponent<PlayerSafeGround>();
        if (hp == null || safe == null) return;

        if (hp.currentHealth <= 0f || safe.IsInvulnerable) return;

        hp.TakingDamage(1f, hitSide: 0, applyKnockback: false);
        hp.Die(restoreFullHealth: false, useLastSafe: true);
    }
}
