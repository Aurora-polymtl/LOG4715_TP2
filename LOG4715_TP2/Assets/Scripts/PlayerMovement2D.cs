using UnityEngine;

public class PlayerMove2D : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float m_MaxSpeed = 10f;
    [SerializeField] private float m_PushSpeed = 5f;
    [SerializeField] private float m_JumpForce = 10f;                  // Amount of force added when the player jumps.
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallSlideSpeed = 2f;
    private Rigidbody2D pushingRb;
    private bool isSlidingOnWall;
    private Rigidbody2D m_Rigidbody2D;
    private Animator m_animate;
    private BoxCollider2D m_Collider;
    private Stamina playerStamina;
    private bool isPushing = false;
    private bool nearPushable = false;
    private float playerInitialMass;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_animate = GetComponent<Animator>();
        m_Collider = GetComponent<BoxCollider2D>();
        playerStamina = GetComponent<Stamina>();
        playerInitialMass = m_Rigidbody2D.mass;
    }
    // Update is called once per frame
    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float currentSpeed = (isPushing ? m_PushSpeed : m_MaxSpeed);

        if (isGrounded() && playerStamina.currentStamina < playerStamina.startingStamina)
        {
            playerStamina.Regenerate();
            m_Rigidbody2D.mass = playerInitialMass;
        }

        if (isPushing && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f && pushingRb != null)
        {
            float dir = Mathf.Sign(Input.GetAxis("Horizontal"));
            float objVxAlongPush = pushingRb.linearVelocity.x * dir;

            if (objVxAlongPush > 0.01f)
            {
                if (!playerStamina.Consume(Stamina.PlayerAction.PushObjet))
                {
                    m_Rigidbody2D.mass = 0.01f;
                }
            }
        }

        if (!this.isSlidingOnWall) {
            m_Rigidbody2D.linearVelocity = new Vector2(horizontalInput * currentSpeed, m_Rigidbody2D.linearVelocity.y);
        }
        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector2.one;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector2(-1, 1);
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded())
        {
            Jump();
        }
        m_animate.SetBool("run", horizontalInput != 0);
        m_animate.SetBool("grounded", isGrounded());

        isPushing = nearPushable && Mathf.Abs(horizontalInput) > 0.01f && isGrounded();
        m_animate.SetBool("push", isPushing);

        isWallSliding();

        if(Input.GetKey(KeyCode.Space) && !isGrounded() && isSlidingOnWall)
        {
            if (playerStamina.Consume(Stamina.PlayerAction.WallJump)) Jump();
        }
        m_Rigidbody2D.gravityScale = 3;
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

    private void isWallSliding()
    {
        if(isHittingWall() && !isGrounded()) {
            isSlidingOnWall = true;
            m_Rigidbody2D.linearVelocity = new Vector2(m_Rigidbody2D.linearVelocity.x, Mathf.Clamp(m_Rigidbody2D.linearVelocity.y, -wallSlideSpeed, float.MaxValue));
        } else
        {
            isSlidingOnWall = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("pushable"))
        {
            nearPushable = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("pushable"))
        {
            nearPushable = false;
            isPushing = false;
        }
    }
    public bool IsPlayerIdle()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        return Mathf.Abs(horizontalInput) < 0.01f && isGrounded();
    }
}
