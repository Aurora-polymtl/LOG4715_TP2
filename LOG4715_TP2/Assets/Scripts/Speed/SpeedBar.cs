using UnityEngine;
using UnityEngine.UI;

public class SpeedBar : MonoBehaviour
{
    [SerializeField] private Speed playerSpeed;
    [SerializeField] private Image currentSpeedBar;

    private void Update()
    {
        if (playerSpeed == null || currentSpeedBar == null) return;
        currentSpeedBar.fillAmount = playerSpeed.currentSpeed / playerSpeed.startingSpeed;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}