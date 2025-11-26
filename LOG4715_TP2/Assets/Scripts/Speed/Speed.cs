using UnityEngine;

public class Speed : MonoBehaviour
{
    [SerializeField] public float startingSpeed = 10f;
    [SerializeField] public float consumptionPerSecond = 5f;
    [SerializeField] private SpeedBar speedBar;
    [SerializeField] private AudioClip playerOneSpeedSound;
    [SerializeField] private AudioClip playerAllSpeedSound;

    public float currentSpeed { get; private set; }
    public int nSpeedPowerUpPickedUp = 0;


    private void Awake()
    {
        currentSpeed = 0f;
    }
    private void Update()
    {
        if (currentSpeed <= 0f) return;

        currentSpeed = Mathf.Max(0f, currentSpeed - consumptionPerSecond * Time.deltaTime);
        if (currentSpeed <= 0f) speedBar.Hide();
    }

    public void AddFragment()
    {
        nSpeedPowerUpPickedUp++;
        SoundManager.instance.PlaySound(playerOneSpeedSound);

        if (nSpeedPowerUpPickedUp >= 3 && currentSpeed <= 0f)
        {
            nSpeedPowerUpPickedUp = 0;
            currentSpeed = startingSpeed;
            speedBar.Show();
            SoundManager.instance.PlaySound(playerAllSpeedSound);
        }
    }
}
