using UnityEngine;

public class PlayerMove2D : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float m_MaxSpeed = 10f;
    [SerializeField] private float m_JumpForce = 10f;                  // Amount of force added when the player jumps.
    [SerializeField] private int midair_Jumps = 1;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private float currentJumpForce;
    private Rigidbody2D m_Rigidbody2D;
    private Animator m_animate;
    private BoxCollider2D m_Collider;
    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_animate = GetComponent<Animator>();
        m_Collider = GetComponent<BoxCollider2D>();
    }
    // Update is called once per frame
    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        m_Rigidbody2D.linearVelocity = new Vector2(horizontalInput * m_MaxSpeed, m_Rigidbody2D.linearVelocity.y);

        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded())
        {
            Jump();
        }
        m_animate.SetBool("run", horizontalInput != 0);
        m_animate.SetBool("grounded", isGrounded());
    }

    private void Jump()
    {
        m_Rigidbody2D.linearVelocity = new Vector2(m_Rigidbody2D.linearVelocity.x, m_JumpForce);
        m_animate.SetTrigger("jump");
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(m_Collider.bounds.center, m_Collider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool isHittingWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(m_Collider.bounds.center, m_Collider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
}
