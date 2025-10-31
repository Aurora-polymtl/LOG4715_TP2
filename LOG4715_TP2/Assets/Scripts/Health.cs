using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float healthStart;
    public float currentHealth { get; private set; }
    private void Awake()
    {
        currentHealth = healthStart;
    }

    public void TakingDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, healthStart);

        if (currentHealth > 0) {
        } else
        {

        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakingDamage(1);
        }
    }

}
