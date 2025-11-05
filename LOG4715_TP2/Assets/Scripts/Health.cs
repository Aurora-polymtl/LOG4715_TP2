using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float healthStart;
    public float currentHealth { get; private set; }
    private Animator m_animate;
    private bool dead;
    private Knockback knockback;
    private void Awake()
    {
        currentHealth = healthStart;
        m_animate = GetComponent<Animator>();
        knockback = GetComponent<Knockback>();

    }

    public void TakingDamage(float _damage, int hitSide)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, healthStart);

        if (currentHealth > 0)
        {
            m_animate.SetTrigger("hurt");
            if (hitSide == -1)
            {
                knockback.CallKnockback(new Vector2(hitSide, 0f), new Vector2(-1, 0.1f));
            }
            else
            {
                knockback.CallKnockback(new Vector2(hitSide, 0f), new Vector2(1, 0.1f));
            }
        }
        else
        {
            if (!dead)
            {
                m_animate.SetTrigger("death");
                GetComponent<PlayerMove2D>().enabled = false;
                dead = true;
            }
        }
    }
}
