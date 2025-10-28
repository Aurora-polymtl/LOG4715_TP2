using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float health;
    private float currentHealth;
    private void Awake()
    {
        currentHealth = health;
    }

    public void TakingDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, health);

        if (currentHealth > 0) {

        } else
        {

        }
    }

}
