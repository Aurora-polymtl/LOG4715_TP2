using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Health : MonoBehaviour
{
    [SerializeField] private float healthStart = 3f;
    public float currentHealth { get; private set; }

    Animator anim;
    Knockback knockback;
    RespawnController respawnController;
    [Header("Death Movement Delay")]
    [SerializeField]
    [Tooltip("Seconds to disable player movement after dying")]
    private float movementDisableAfterDeath = 2f;
    [Header("Damage Invulnerability")]
    [SerializeField]
    [Tooltip("Seconds the object is invulnerable after taking damage")]
    private float damageInvulnerability = 2.5f;

    public bool IsInvulnerable { get; private set; }
    private Coroutine invulCoroutine;
    bool dead;

    void Awake()
    {
        currentHealth = healthStart;
        anim = GetComponent<Animator>();
        knockback = GetComponent<Knockback>();
        respawnController = RespawnController.instance;
        IsInvulnerable = false;
    }

    public void TriggerInvulnerability(float duration)
    {
        if (invulCoroutine != null)
            StopCoroutine(invulCoroutine);
        invulCoroutine = StartCoroutine(InvulnerabilityRoutine(duration));
    }

    private System.Collections.IEnumerator InvulnerabilityRoutine(float duration)
    {
        IsInvulnerable = true;
        yield return new WaitForSeconds(duration);
        IsInvulnerable = false;
        invulCoroutine = null;
    }

    public void TakingDamage(float amount, int hitSide, bool applyKnockback)
    {
        if (IsInvulnerable) return;

        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, healthStart);

        if (currentHealth > 0f)
        {
            anim.SetTrigger("hurt");
            if (applyKnockback)
                knockback.CallKnockback(hitSide >= 0 ? 1 : -1);
            // start temporary invulnerability to prevent repeated hits
            TriggerInvulnerability(damageInvulnerability);
        }
        else if (!dead)
        {
            Die(restoreFullHealth: true, useLastSafe: false);
        }
    }

    public void Die(bool restoreFullHealth, bool useLastSafe = true)
    {
        print("Player died.");
        if (dead) return;
        dead = true;

        var controller = GetComponent<PlayerMove2D>();
        if (controller) controller.enabled = false;

        if (!restoreFullHealth && useLastSafe)
        {
            // anim.SetTrigger("death");
            var safe = GetComponent<PlayerSafeGround>();
            if (safe != null)
            {
                print("Teleporting to last safe ground.");
                safe.TeleportToLastSafe(ignoreHazard: true, ignoreDuration: 0.6f);
            }
            else
            {
                if (respawnController != null) respawnController.RespawnPlayer();
            }
        }
        else
        {
            currentHealth = healthStart;
            if (respawnController != null) respawnController.RespawnPlayer();
        }
        anim.Play("Idle", 0, 0f);

        // Keep movement disabled for a short duration after respawn, then re-enable
        if (controller)
        {
            // start coroutine to re-enable movement after configured delay
            StartCoroutine(ReenableMovementAfterDelay(controller, movementDisableAfterDeath));
        }
        else
        {
            // no controller; just clear dead flag
            dead = false;
        }
    }

    private System.Collections.IEnumerator ReenableMovementAfterDelay(PlayerMove2D controller, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (controller) controller.enabled = true;
        dead = false;
    }
}
