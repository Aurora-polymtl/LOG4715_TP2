using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IResettable
{
    void ResetState();
}

public class ResetObjectStateManager : MonoBehaviour
{
    public static ResetObjectStateManager instance { get; private set; }

    private List<IResettable> resettables;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        resettables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IResettable>()
            .ToList();

    }

    public void ResetAll()
    {
        foreach (var r in resettables)
            r.ResetState();
    }
}
