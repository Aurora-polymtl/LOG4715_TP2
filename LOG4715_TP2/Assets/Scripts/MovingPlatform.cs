using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum MovementType { Linear, ZigZag, Circular }

    [SerializeField] private MovementType movementType = MovementType.Linear;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float distance = 3f; // Pour linear/zigzag
    [SerializeField] private float radius = 2f;   // Pour circular
    [SerializeField] private Vector2 direction = Vector2.right; // Sens du mouvement

    private Vector3 startPos;
    private float timeOffset;

    private void Start()
    {
        startPos = transform.position;
        timeOffset = Random.Range(0f, 100f); // Décale le mouvement si plusieurs plateformes
    }

    private void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        switch (movementType)
        {
            case MovementType.Linear:
                transform.position = startPos + (Vector3)direction.normalized * Mathf.Sin((Time.time + timeOffset) * speed) * distance;
                break;

            case MovementType.ZigZag:
                // Mouvement en "L" (alternance horizontale et verticale)
                float x = Mathf.Sin((Time.time + timeOffset) * speed) * distance;
                float y = Mathf.Sin((Time.time + timeOffset) * speed * 0.5f) * distance * 0.5f;
                transform.position = startPos + new Vector3(x, y, 0);
                break;

            case MovementType.Circular:
                float angle = (Time.time + timeOffset) * speed;
                transform.position = startPos + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
                break;
        }
    }

    // --- Gestion du joueur sur la plateforme ---
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMove2D player = collision.gameObject.GetComponent<PlayerMove2D>();
            if (player != null)
            {
                if (player.IsPlayerIdle())
                {
                    if (collision.transform.parent != transform)
                        collision.transform.SetParent(transform);
                }
                else
                {
                    if (collision.transform.parent == transform)
                        collision.transform.SetParent(null);
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // On détache le joueur quand il quitte la plateforme
            collision.transform.SetParent(null);
        }
    }
}
