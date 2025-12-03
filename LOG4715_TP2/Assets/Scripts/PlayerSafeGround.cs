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

    [Header("Safe position buffering")]
    [SerializeField]
    [Tooltip("Number of FixedUpdate samples to keep in the buffer (higher = older safe positions available)")]
    private int safePositionBufferSize = 12;

    [SerializeField]
    [Tooltip("How many samples back to use as the 'last safe' position. For example, 3 means lastSafePos = position from 3 FixedUpdate steps ago.")]
    private int safePositionLookback = 3;

    public Vector2 lastSafePos { get; private set; }
    public bool IsInvulnerable { get; private set; }

    Rigidbody2D rb;
    Collider2D col;

    int playerLayer = -1, hazardLayerIdx = -1;
    Coroutine ignoreRoutine;
    private System.Collections.Generic.List<Vector2> safePositions = new System.Collections.Generic.List<Vector2>();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        lastSafePos = rb.position;

        playerLayer = LayerMask.NameToLayer(playerLayerName);
        hazardLayerIdx = LayerMask.NameToLayer(hazardLayerName);
    }

    void FixedUpdate()
    {
        if (!IsInvulnerable && IsGrounded() && !HazardJustBelow())
        {
            safePositions.Add(rb.position);
            if (safePositions.Count > safePositionBufferSize)
                safePositions.RemoveAt(0);

            if (safePositions.Count > safePositionLookback)
            {
                int index = safePositions.Count - 1 - safePositionLookback;
                lastSafePos = safePositions[index];
            }
        }
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
        float dir = Mathf.Sign(transform.position.x - lastSafePos.x);
        if (dir == 0f)
        {
            dir = Mathf.Sign(transform.localScale.x);
            if (dir == 0f) dir = 1f;
        }
        float xOff = dir * teleportXOffset;
        TeleportTo(lastSafePos + new Vector2(xOff, teleportYOffset), ignoreHazard, ignoreDuration);
    }

    public void teleportToRoomStart(bool ignoreHazard = true, float ignoreDuration = 0.6f)
    {
        // 1. HARDCODE : On cherche l'objet par son nom exact dans la scène
        GameObject targetObj = GameObject.Find("respawn hasard");

        if (targetObj != null)
        {
            // 2. Si on le trouve, on se téléporte directement à sa position
            TeleportTo(targetObj.transform.position, ignoreHazard: false);
        }
        // else
        // {
        //     // 3. Sécurité : Si l'objet n'existe pas ou est mal nommé, on garde l'ancien comportement pour ne pas bloquer le jeu
        //     Debug.LogWarning("Attention : L'objet 'respawn hasard' est introuvable dans la scène !");
        //     Vector2 target = roomStartPos + new Vector2(teleportXOffset, teleportYOffset);
        //     TeleportTo(target, ignoreHazard: false);
        // }
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
        safePositions.Clear();
        safePositions.Add(lastSafePos);

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
