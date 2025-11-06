using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Knockback : MonoBehaviour
{
    [Header("Uniform knockback")]
    [Tooltip("Durée pendant laquelle le knockback est imposé")]
    public float duration = 0.18f;

    [Tooltip("Vitesse horizontale (constante) appliquée pendant le knockback")]
    public float horizontalSpeed = 6f;

    [Tooltip("Impulsion verticale initiale")]
    public float upwardImpulse = 2.5f;

    public bool IsBeingKnockedBack { get; private set; }

    Rigidbody2D rb;
    Coroutine routine;

    void Awake() { rb = GetComponent<Rigidbody2D>(); }

    public void CallKnockback(int side)
    {
        if (side == 0) side = 1;
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(RunKnockback(side));
    }

    IEnumerator RunKnockback(int side)
    {
        IsBeingKnockedBack = true;

        rb.linearVelocity = new Vector2(side * horizontalSpeed, upwardImpulse);

        float t = 0f;
        while (t < duration)
        {
            t += Time.fixedDeltaTime;
            rb.linearVelocity = new Vector2(side * horizontalSpeed, rb.linearVelocity.y);
            yield return new WaitForFixedUpdate();
        }

        IsBeingKnockedBack = false;
        routine = null;
    }
}
