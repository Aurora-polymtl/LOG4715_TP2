using UnityEngine;
using System.Collections;

public class Speed : MonoBehaviour
{
    [SerializeField] public float startingSpeed = 10f;
    [SerializeField] public float consumptionPerSecond = 5f;
    [SerializeField] private SpeedBar speedBar;
    [SerializeField] private float respawnTime = 5f;
    private bool isRespawning = false;
    public float currentSpeed { get; private set; }
    public int nSpeedPowerUpPickedUp = 0;


    private void Awake()
    {
        currentSpeed = 0f;
    }
    private void Update()
    {
        if (currentSpeed > 0f)
        {
            currentSpeed = Mathf.Max(0f, currentSpeed - consumptionPerSecond * Time.deltaTime);
            if (currentSpeed <= 0f && !isRespawning)
            {
                isRespawning = true;
                speedBar.Hide();
                StartCoroutine(RespawnAfterDelay());
            }
        }
    }

    public void PickupFragment()
    {
        nSpeedPowerUpPickedUp++;

        if (nSpeedPowerUpPickedUp >= 3 && currentSpeed <= 0f)
        {
            nSpeedPowerUpPickedUp = 0;
            currentSpeed = startingSpeed;
            speedBar.Show();
        }
    }

    public IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnTime);
        PowerUpManager.Instance.RespawnAll();
        isRespawning = false;
    }
}
