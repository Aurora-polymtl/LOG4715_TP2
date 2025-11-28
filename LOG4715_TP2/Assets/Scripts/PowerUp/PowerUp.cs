using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private void Start()
    {
        if (PowerUpManager.Instance != null)
        {
            PowerUpManager.Instance.Register(gameObject);
        }
    }
}
