using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float healthStart = 3f;
    public float currentHealth { get; private set; }

    Animator anim;
    Knockback knockback;
    bool dead;

    void Awake()
    {
        currentHealth = healthStart;
        anim = GetComponent<Animator>();
        knockback = GetComponent<Knockback>();
    }

    public void TakingDamage(float amount, int hitSide, bool applyKnockback = true)
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
            anim.SetTrigger("death");
            GetComponent<PlayerMove2D>().enabled = false;
            dead = true;
        }
    }

}
