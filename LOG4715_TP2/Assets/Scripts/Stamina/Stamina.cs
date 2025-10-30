using UnityEngine;
using System.Collections.Generic;

public class Stamina : MonoBehaviour
{
    [SerializeField] public float startingStamina { get; private set; } = 10f;
    [SerializeField] public float regenerationPerSecond = 5f;
    public float currentStamina { get; private set; }

    public enum PlayerAction
    {
        WallJump,
        Dash,
        PushObjet
    }

    public Dictionary<PlayerAction, float> staminaCosts = new Dictionary<PlayerAction, float>
    {
        { PlayerAction.WallJump, 0.25f },
        { PlayerAction.Dash, 0.10f },
        { PlayerAction.PushObjet, 0.20f}
    };

    private void Awake()
    {
        currentStamina = startingStamina;
    }

    public bool Consume(PlayerAction action) 
    {
        float cost = staminaCosts[action];
        if (currentStamina < cost) return false;
        currentStamina -= cost;
        return true;
    }

    public void Regenerate()
    {
        currentStamina = Mathf.Min(currentStamina + regenerationPerSecond * Time.deltaTime, startingStamina);
    }
}
