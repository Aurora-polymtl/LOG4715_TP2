using UnityEngine;
using System.Collections.Generic;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    private List<GameObject> powerUps = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public void Register(GameObject p)
    {
        powerUps.Add(p);
    }

    public void RespawnAll()
    {
        foreach (var p in powerUps)
            p.SetActive(true);
    }
}
