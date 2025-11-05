using System.Collections;
using UnityEngine;

public class PlayerMove2D : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float m_MaxSpeed = 10f;
    [SerializeField] private float m_JumpForce = 10f;                  // Amount of force added when the player jumps.
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallSlideSpeed = 2f;
    private Rigidbody2D pushingRb;
    private bool isSlidingOnWall;
    private Rigidbody2D m_Rigidbody2D;
    private Animator m_animate;
    private BoxCollider2D m_Collider;
    private bool canDash;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    private Stamina playerStamina;
    private bool isPushing;
    private Knockback knockback;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_animate = GetComponent<Animator>();
        m_Collider = GetComponent<BoxCollider2D>();
        // allow dashing when the game starts
        canDash = true;
        playerStamina = GetComponent<Stamina>();
        knockback = GetComponent<Knockback>();
    }
    // Update is called once per frame
    private void Update()
    {
        if (isDashing)
        {
            return;
        }
        float horizontalInput = Input.GetAxis("Horizontal");

        if (isGrounded() && playerStamina.currentStamina < playerStamina.startingStamina)
        {
            playerStamina.Regenerate();
        }

        if (isPushing && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f && pushingRb != null)
        {
            float dir = Mathf.Sign(Input.GetAxis("Horizontal"));
            float objVxAlongPush = pushingRb.linearVelocity.x * dir;

            if (objVxAlongPush > 0.01f)
                playerStamina.Consume(Stamina.PlayerAction.PushObjet);
        }

        if (!knockback.IsBeingKnockedBack)
        {
            if (!this.isSlidingOnWall)
            {
                m_Rigidbody2D.linearVelocity = new Vector2(horizontalInput * m_MaxSpeed, m_Rigidbody2D.linearVelocity.y);
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
        }

        m_animate.SetBool("grounded", isGrounded());

        isWallSliding();

        if (Input.GetKey(KeyCode.Space) && !isGrounded() && isSlidingOnWall)
        {
            if (playerStamina.Consume(Stamina.PlayerAction.WallJump))
                Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        m_Rigidbody2D.gravityScale = 3;
    }

    private void Jump()
    {
        m_Rigidbody2D.linearVelocity = new Vector2(m_Rigidbody2D.linearVelocity.x, m_JumpForce);
        m_animate.SetTrigger("jump");
    }

    private IEnumerator Dash()
    {
        Physics2D.IgnoreLayerCollision(10, 9, true);
        canDash = false;
        isDashing = true;
        float originalGravity = m_Rigidbody2D.gravityScale;
        m_Rigidbody2D.gravityScale = 0f;
        m_Rigidbody2D.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        m_Rigidbody2D.gravityScale = originalGravity;
        isDashing = false;
        Physics2D.IgnoreLayerCollision(10, 9, false);

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;

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
        if (isHittingWall() && !isGrounded())
        {
            isSlidingOnWall = true;
            m_Rigidbody2D.linearVelocity = new Vector2(m_Rigidbody2D.linearVelocity.x, Mathf.Clamp(m_Rigidbody2D.linearVelocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            isSlidingOnWall = false;
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        var rb = col.rigidbody;
        if (rb == null || rb.bodyType != RigidbodyType2D.Dynamic) return;

        Vector2 normal = col.GetContact(0).normal;

        if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
        {
            isPushing = true;
            pushingRb = rb;
        }
        else
        {
            isPushing = false;
            pushingRb = null;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.rigidbody == pushingRb)
        {
            isPushing = false;
            pushingRb = null;
        }
    }
}
