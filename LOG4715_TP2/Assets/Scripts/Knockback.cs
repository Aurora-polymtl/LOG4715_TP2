using System.Collections;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    // Tunable defaults (reduced to make knockback less strong)
    public float knockbackTime = 0.18f;
    public float hitDirectionForce = 4f;
    public float constForce = 2f;
    public float inputForce = 2.5f;
    public bool IsBeingKnockedBack { get; private set; }
    private Rigidbody2D rb;
    private Coroutine knockbackCoroutine;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }




    public IEnumerator KnockbackAction(Vector2 hitDirection, Vector2 constantForceDirection, float inputDirection)
    {
        IsBeingKnockedBack = true;
        Vector2 _hitforce;
        Vector2 _constantForce;
        Vector2 _knockbackForce;
        Vector2 _combinedForce;

        _hitforce = hitDirection * hitDirectionForce;
        _constantForce = constantForceDirection * constForce;

        float _elapsedTime = 0f;
        while (_elapsedTime < knockbackTime)
        {
            _elapsedTime += Time.fixedDeltaTime;

            _knockbackForce = _hitforce + _constantForce;

            if (inputDirection != 0)
            {
                _combinedForce = _knockbackForce + new Vector2(inputDirection * inputForce, 0f);
            }
            else
            {
                _combinedForce = _knockbackForce;
            }
            rb.linearVelocity = _combinedForce;
            yield return new WaitForFixedUpdate();
        }
        IsBeingKnockedBack = false;

    }

    public void CallKnockback(Vector2 hitDirection, Vector2 constantForceDirection, float inputDirection)
    {
        knockbackCoroutine = StartCoroutine(KnockbackAction(hitDirection, constantForceDirection, inputDirection));
    }
}