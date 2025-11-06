using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Health : MonoBehaviour
{
    [SerializeField] private float healthStart = 3f;
    public float currentHealth { get; private set; }

    Animator anim;
    Knockback knockback;
    RespawnController respawnController;
    bool dead;

    void Awake()
    {
        currentHealth = healthStart;
        anim = GetComponent<Animator>();
        knockback = GetComponent<Knockback>();
        respawnController = RespawnController.instance
                          ?? Object.FindFirstObjectByType<RespawnController>(FindObjectsInactive.Exclude);
    }

    public void TakingDamage(float amount, int hitSide, bool applyKnockback)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, healthStart);

        if (currentHealth > 0f)
        {
            anim.SetTrigger("hurt");
            if (applyKnockback)
                knockback.CallKnockback(hitSide >= 0 ? 1 : -1);
        }
        else if (!dead)
        {
            Die(restoreFullHealth: true, useLastSafe: false);
        }
    }

    public void Die(bool restoreFullHealth, bool useLastSafe = true)
    {
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
                safe.TeleportToLastSafe(ignoreHazard: true, ignoreDuration: 0.6f);
            }
            else
            {
                EnsureController();
                if (respawnController != null) respawnController.RespawnPlayer();
            }
        }
        else
        {
            currentHealth = healthStart;
            EnsureController();
            if (respawnController != null) respawnController.RespawnPlayer();
        }
        anim.Play("Idle", 0, 0f);
        if (controller) controller.enabled = true;

        dead = false;
    }

    void EnsureController()
    {
        if (respawnController == null)
            respawnController = RespawnController.instance
                             ?? Object.FindFirstObjectByType<RespawnController>(FindObjectsInactive.Exclude);
        if (respawnController == null)
            Debug.LogError("[Health] Aucun RespawnController dans la scène.");
    }
}
