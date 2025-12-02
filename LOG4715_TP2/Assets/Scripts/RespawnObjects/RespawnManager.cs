using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager instance;

    private List<Respawnable> list = new();

    private void Awake()
    {
        instance = this;
    }

    public void Register(Respawnable r)
    {
        list.Add(r);
    }

    public void RespawnAll()
    {
        foreach (var r in list)
            r.Respawn();
    }
}
