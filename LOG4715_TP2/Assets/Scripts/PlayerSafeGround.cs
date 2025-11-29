using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerSafeGround : MonoBehaviour
{
    [Header("Masks utilisés par les BoxCast")]
    public LayerMask groundLayer;
    public LayerMask hazardLayer;

    [Header("Layers utilisés par IgnoreLayerCollision")]
    [SerializeField] private string playerLayerName = "player";
    [SerializeField] private string hazardLayerName = "Hazard";

    [Header("Offsets de TP")]
    [SerializeField] private float teleportXOffset = 0.5f;
    [SerializeField] private float teleportYOffset = 0.5f;

    public Vector2 lastSafePos { get; private set; }
    public bool IsInvulnerable { get; private set; }

    Rigidbody2D rb;
    Collider2D col;

    int playerLayer = -1, hazardLayerIdx = -1;
    Coroutine ignoreRoutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        lastSafePos = rb.position;

        playerLayer = LayerMask.NameToLayer(playerLayerName);
        hazardLayerIdx = LayerMask.NameToLayer(hazardLayerName);
        if (playerLayer == -1) Debug.LogWarning($"[PlayerSafeGround] Layer '{playerLayerName}' introuvable.");
        if (hazardLayerIdx == -1) Debug.LogWarning($"[PlayerSafeGround] Layer '{hazardLayerName}' introuvable.");
    }

    void FixedUpdate()
    {
        if (!IsInvulnerable && IsGrounded() && !HazardJustBelow())
            lastSafePos = rb.position;
    }

    bool IsGrounded()
    {
        var hit = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
        return hit.collider != null;
    }

    bool HazardJustBelow()
    {
        var hit = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, 0.11f, hazardLayer);
        return hit.collider != null;
    }


    public void ForceSetLastSafe(Vector2 pos) => lastSafePos = pos;

    public void TeleportTo(Vector2 target, bool ignoreHazard = true, float ignoreDuration = 0.6f)
    {
        rb.linearVelocity = Vector2.zero;
        if (ignoreRoutine != null) StopCoroutine(ignoreRoutine);
        ignoreRoutine = StartCoroutine(DoTeleport(target, ignoreHazard ? ignoreDuration : 0f));
    }

    public void TeleportToLastSafe(bool ignoreHazard = true, float ignoreDuration = 0.6f)
    {
        TeleportTo(lastSafePos + new Vector2(-teleportXOffset, 0), ignoreHazard, ignoreDuration);
    }

    private IEnumerator DoTeleport(Vector2 target, float ignoreDuration)
    {
        IsInvulnerable = ignoreDuration > 0f;

        col.enabled = false;
        rb.position = target;
        yield return new WaitForFixedUpdate();
        col.enabled = true;
        rb.linearVelocity = Vector2.zero;

        lastSafePos = rb.position;

        if (playerLayer != -1 && hazardLayerIdx != -1)
            Physics2D.IgnoreLayerCollision(playerLayer, hazardLayerIdx, ignoreDuration > 0f);

        if (ignoreDuration > 0f)
        {
            yield return new WaitForSeconds(ignoreDuration);
            if (playerLayer != -1 && hazardLayerIdx != -1)
                Physics2D.IgnoreLayerCollision(playerLayer, hazardLayerIdx, false);
            IsInvulnerable = false;
        }
        ignoreRoutine = null;
    }
}
