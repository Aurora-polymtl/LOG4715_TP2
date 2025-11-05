using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            float dx = collision.transform.position.x - transform.position.x;
            int hitSide = dx == 0 ? 0 : (dx > 0 ? 1 : -1);
            collision.GetComponent<Health>().TakingDamage(damage, hitSide);
        }
    }
}
