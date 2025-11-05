using System.Collections;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    // Tunable defaults (reduced to make knockback less strong)
    public float knockbackTime = 0.18f;
    public float hitDirectionForce = 4f;
    public float constForce = 2f;
    public bool IsBeingKnockedBack { get; private set; }
    private Rigidbody2D rb;
    private Coroutine knockbackCoroutine;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }




    public IEnumerator KnockbackAction(Vector2 hitDirection, Vector2 constantForceDirection)
    {
        IsBeingKnockedBack = true;
        Vector2 _hitforce;
        Vector2 _constantForce;
        Vector2 _combinedForce;

        // Use normalized directions so magnitude is controlled only by the *_Force fields
        _hitforce = hitDirection.normalized * hitDirectionForce;
        _constantForce = constantForceDirection.normalized * constForce;

        float _elapsedTime = 0f;
        while (_elapsedTime < knockbackTime)
        {
            _elapsedTime += Time.fixedDeltaTime;

            // Always apply the same attacker-relative knockback; ignore player input
            _combinedForce = _hitforce + _constantForce;
            rb.linearVelocity = _combinedForce;
            yield return new WaitForFixedUpdate();
        }
        IsBeingKnockedBack = false;

    }

    public void CallKnockback(Vector2 hitDirection, Vector2 constantForceDirection)
    {
        knockbackCoroutine = StartCoroutine(KnockbackAction(hitDirection, constantForceDirection));
    }
}