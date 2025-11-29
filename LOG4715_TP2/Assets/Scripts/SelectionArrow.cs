using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    private RectTransform arrowRect;
    private int currentPosition;

    private void Awake()
    {
        arrowRect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            ChangePosition(-1);

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            ChangePosition(1);

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            Interact();
    }

    private void ChangePosition(int change)
    {
        currentPosition += change;

        if (currentPosition < 0)
            currentPosition = options.Length - 1;
        else if (currentPosition >= options.Length)
            currentPosition = 0;

        // Déplacement propre UI
        arrowRect.anchoredPosition = new Vector2(
            arrowRect.anchoredPosition.x,
            options[currentPosition].anchoredPosition.y
        );
    }

    private void Interact()
    {
        Button btn = options[currentPosition].GetComponent<Button>();
        btn.onClick.Invoke();
    }
}
