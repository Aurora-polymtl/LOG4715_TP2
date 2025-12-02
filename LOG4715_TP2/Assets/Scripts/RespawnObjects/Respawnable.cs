using UnityEngine;

public class Respawnable : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void Start()
    {
        if (RespawnManager.instance != null)
        {
            RespawnManager.instance.Register(this);
        }
    }

    public void Respawn()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0;
        }

        gameObject.SetActive(true);
    }
}
