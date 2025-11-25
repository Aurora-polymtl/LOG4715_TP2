using System.Collections;
using UnityEngine;

public class PlayerMove2D : MonoBehaviour
{
    [SerializeField] private float m_MaxSpeed = 10f;
    [SerializeField] private float m_JumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallSlideSpeed = 2f;
    [SerializeField] private AudioClip playerJumpSound;
    [SerializeField] private AudioClip playerDashSound;
    private Rigidbody2D pushingRb;
    private bool isSlidingOnWall;
    private Rigidbody2D m_Rigidbody2D;
    private Animator m_animate;
    private BoxCollider2D m_Collider;
    private bool canDash;
    private bool isDashing;
    private float dashingPower = 22f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    private Stamina playerStamina;
    private Speed playerSpeed;
    private bool isPushing;
    private Knockback knockback;
    private float playerInitialMass;
    [SerializeField] private Vector2 wallJumpPower = new Vector2(6f, 10f);
    [SerializeField] private float wallJumpDuration = 0.2f;
    private bool isWallJumping;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_animate = GetComponent<Animator>();
        m_Collider = GetComponent<BoxCollider2D>();
        canDash = true;
        playerStamina = GetComponent<Stamina>();
        playerSpeed = GetComponent<Speed>();
        playerInitialMass = m_Rigidbody2D.mass;
        knockback = GetComponent<Knockback>();
    }
    private void Update()
    {
        if (isDashing)
        {
            m_Rigidbody2D.gravityScale = 0f;

            return;
        }

        if (!isWallJumping)
        {
            float horizontalInput = Input.GetAxis("Horizontal");

            if (isGrounded() && playerStamina.currentStamina < playerStamina.startingStamina)
            {
                playerStamina.Regenerate();
                m_Rigidbody2D.mass = playerInitialMass;
            }

            if (isPushing && Mathf.Abs(horizontalInput) > 0.1f && pushingRb != null)
            {
                m_animate.SetBool("pushing", true);
                float dir = Mathf.Sign(horizontalInput);
                float objVxAlongPush = pushingRb.linearVelocity.x * dir;

                if (objVxAlongPush > 0.01f)
                {
                    if (!playerStamina.Consume(Stamina.PlayerAction.PushObjet))
                    {
                        m_animate.SetBool("pushing", false);
                        m_Rigidbody2D.mass = 0.01f;
                    }
                }
            }
            else
            {
                m_animate.SetBool("pushing", false);
            }

            if (!knockback.IsBeingKnockedBack)
            {
                if (!this.isSlidingOnWall)
                {
                    float moveSpeed = m_MaxSpeed;
                    if (playerSpeed.currentSpeed > 0f) moveSpeed *= 2f;

                    m_Rigidbody2D.linearVelocity = new Vector2(horizontalInput * moveSpeed, m_Rigidbody2D.linearVelocity.y);
                }

                if (horizontalInput > 0.01f)
                {
                    transform.localScale = Vector2.one;
                }
                else if (horizontalInput < -0.01f)
                {
                    transform.localScale = new Vector2(-1, 1);
                }

                if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
                {
                    Jump();
                }

                m_animate.SetBool("run", horizontalInput != 0);
            }
        }

        m_animate.SetBool("grounded", isGrounded());

        isWallSliding();

        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded() && isSlidingOnWall)
        {
            if (playerStamina.Consume(Stamina.PlayerAction.WallJump))
            {
                WallJump();
                if (Input.GetKeyDown(KeyCode.Space) && !isGrounded())
                    SoundManager.instance.PlaySound(playerJumpSound);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            if (playerStamina.Consume(Stamina.PlayerAction.Dash))
            {
                StartCoroutine(Dash());
                SoundManager.instance.PlaySound(playerDashSound);
            }
        }
        m_Rigidbody2D.gravityScale = 3;
    }

    private void Jump()
    {
        m_Rigidbody2D.linearVelocity = new Vector2(m_Rigidbody2D.linearVelocity.x, m_JumpForce);
        m_animate.SetTrigger("jump");
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            SoundManager.instance.PlaySound(playerJumpSound);
        }
    }

    private void WallJump()
    {
        isWallJumping = true;
        float wallDirection = -Mathf.Sign(transform.localScale.x);
        m_Rigidbody2D.linearVelocity = new Vector2(wallDirection * wallJumpPower.x, wallJumpPower.y);
        transform.localScale = new Vector2(wallDirection, 1f);
        m_animate.SetTrigger("jump");
        Invoke(nameof(StopWallJump), wallJumpDuration);
    }

    private void StopWallJump()
    {
        isWallJumping = false;
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

    private void OnTriggerEnter2D(Collider2D powerUp)
    {
        if (powerUp.CompareTag("PowerUp"))
        {
            playerSpeed.AddFragment();
            Destroy(powerUp.gameObject);
        }
    }

    public bool IsPlayerIdle()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        return Mathf.Abs(horizontalInput) < 0.01f && isGrounded();
    }
}
