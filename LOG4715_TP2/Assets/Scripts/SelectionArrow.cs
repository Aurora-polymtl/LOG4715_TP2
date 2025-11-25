using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private RectTransform[] options;
    private RectTransform m_RectTransform;
    private int currentPosition;

    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            ChangePosition(-1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            ChangePosition(1);
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Interact();
        }
    }
    private void ChangePosition(int change)
    {
        currentPosition += change;
        if (currentPosition < 0)
        {
            currentPosition = options.Length - 1;
        }
        else if (currentPosition > options.Length - 1) {
            currentPosition = 0;
        }
        m_RectTransform.position = new Vector2(m_RectTransform.position.x, options[currentPosition].position.y);
    }

    private void Interact()
    {
        options[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
}
