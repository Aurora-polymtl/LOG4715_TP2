using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum MovementType { Linear, ZigZag, Circular, Vertical, VerticalLongWall }

    [SerializeField] private MovementType movementType = MovementType.Linear;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float distance = 3f; // Pour linear/zigzag
    [SerializeField] private float radius = 2f;   // Pour circular
    [SerializeField] private float verticalDistance = 2f; // Pour linear/zigzag
    [SerializeField] private Vector2 direction = Vector2.right; // Sens du mouvement
    [SerializeField] private Vector2 upDirection = Vector2.up; // Sens du mouvement

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

            case MovementType.Vertical:
                transform.position = startPos + (Vector3)upDirection.normalized * Mathf.Cos((Time.time + timeOffset) * speed) * (verticalDistance);
                break;

            case MovementType.VerticalLongWall:
                transform.position = startPos + (Vector3)upDirection.normalized * Mathf.Cos((Time.time + timeOffset) * speed * 0.75f) * (verticalDistance * 1.65f);
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
