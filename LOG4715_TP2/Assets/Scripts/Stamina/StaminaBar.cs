using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private Stamina playerStamina;
    [SerializeField] private Image currentStaminaBar;

    private void Update()
    {
        currentStaminaBar.fillAmount = playerStamina.currentStamina / playerStamina.startingStamina;
    }
}
