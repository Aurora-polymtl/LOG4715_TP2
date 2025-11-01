using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float healthStart;
    public float currentHealth { get; private set; }
    private Animator m_animate;
    private bool dead;
    private void Awake()
    {
        currentHealth = healthStart;
        m_animate = GetComponent<Animator>();
    }

    public void TakingDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, healthStart);

        if (currentHealth > 0) {
            m_animate.SetTrigger("hurt");
        } else
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
