using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private float damage = 1f;
    [SerializeField] private bool useTrigger = true;

    private Collider2D self;

    void Awake() => self = GetComponent<Collider2D>();

    void OnTriggerEnter2D(Collider2D other) { if (useTrigger) HandleHit(other); }
    void OnCollisionEnter2D(Collision2D c) { if (!useTrigger) HandleHit(c.collider); }

    private void HandleHit(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Vector2 from = self.bounds.center;
        Vector2 to = other.ClosestPoint(from);
        float dx = to.x - from.x;

        if (Mathf.Abs(dx) < 0.01f)
        {
            var rb = other.attachedRigidbody;
            if (rb != null && Mathf.Abs(rb.linearVelocity.x) > 0.01f) dx = rb.linearVelocity.x;
            else dx = other.bounds.center.x - from.x;
        }
        int hitSide = dx >= 0 ? 1 : -1;

        var hp = other.GetComponent<Health>();
        if (hp != null)
            hp.TakingDamage(damage, hitSide, true);
    }
}
