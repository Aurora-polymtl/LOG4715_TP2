using UnityEngine;
using UnityEngine.Events;

public enum LeverState
{
    Left, Center, Right
}

public class Lever : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite lever_center;
    public Sprite lever_left;
    public Sprite lever_right;

    [Header("État du levier")]
    public LeverState state = LeverState.Left;

    private SpriteRenderer render;
    private LeverState prev_state;
    private bool canActivate = false;
    private float cooldown = 0f;

    public UnityEvent OnTriggerLever;

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        prev_state = state;
        ChangeSprite();
    }

    void Update()
    {
        // Gestion du délai entre activations
        if (cooldown > 0f)
            cooldown -= Time.deltaTime;

        // Actualiser le sprite si l'état change
        if (state != prev_state)
        {
            ChangeSprite();
            prev_state = state;
        }

        // Interaction joueur
        if (canActivate && cooldown <= 0f)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                Activate();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            canActivate = true;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            canActivate = false;
        }
    }

    public void Activate()
    {
        cooldown = 0.8f; // empêche l'activation trop rapide

        if (state == LeverState.Left)
            state = LeverState.Right;
        else if (state == LeverState.Right)
            state = LeverState.Left;
        else
            state = LeverState.Right; // si le levier était au centre

        // Déclencher les événements liés au levier
        OnTriggerLever?.Invoke();
    }

    private void ChangeSprite()
    {
        switch (state)
        {
            case LeverState.Left:
                render.sprite = lever_left;
                break;
            case LeverState.Center:
                render.sprite = lever_center;
                break;
            case LeverState.Right:
                render.sprite = lever_right;
                break;
        }
    }
}
