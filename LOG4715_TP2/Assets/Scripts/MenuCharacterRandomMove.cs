using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class MenuCharacterRandomMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float maxSpeed = 3f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float wallSlideSpeed = 2f;

    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D col;

    private bool isGrounded;
    private bool isWallSliding;
    private bool isJumping;
    private float direction = 1f; // 1 = right, -1 = left

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        StartCoroutine(RandomMovementRoutine());
    }

    private void Update()
    {
        CheckGrounded();
        WallSlideCheck();

        // Appliquer vitesse horizontale
        rb.linearVelocity = new Vector2(direction * maxSpeed, rb.linearVelocity.y);

        // Update animations
        animator.SetBool("run", Mathf.Abs(direction) > 0.01f);
        animator.SetBool("grounded", isGrounded);

        if (isJumping && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger("jump");
            isJumping = false;
        }
    }

    private void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
        isGrounded = hit.collider != null;
    }

    private void WallSlideCheck()
    {
        RaycastHit2D hit = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, new Vector2(direction, 0), 0.1f, groundLayer);
        isWallSliding = hit.collider != null && !isGrounded;

        if (isWallSliding)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlideSpeed, float.MaxValue));
            direction *= -1; // rebondir en changeant de direction
            Flip();
        }
    }

    private IEnumerator RandomMovementRoutine()
    {
        while (true)
        {
            // Choisir un temps al�atoire avant la prochaine action
            float wait = Random.Range(1f, 3f);
            yield return new WaitForSeconds(wait);

            // Choisir une action al�atoire
            int action = Random.Range(0, 3); // 0=idle, 1=walk, 2=jump

            switch (action)
            {
                case 0: // idle
                    direction = 0;
                    break;
                case 1: // walk
                    direction = Random.value > 0.5f ? 1f : -1f;
                    Flip();
                    break;
                case 2: // jump
                    if (isGrounded)
                        isJumping = true;
                    break;
            }
        }
    }

    private void Flip()
    {
        if (direction != 0)
            transform.localScale = new Vector3(Mathf.Sign(direction), 1f, 1f);
    }
}
