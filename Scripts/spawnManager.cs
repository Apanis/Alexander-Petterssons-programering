using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnManager : MonoBehaviour
{
    public static spawnManager Instance;

    spawnPoint[] spawnPoints;
    void Awake()
    {
        Instance = this;
        spawnPoints = GetComponentsInChildren<spawnPoint>();
    }
    public Transform GetSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
    }
}
