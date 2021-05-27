using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    public EnvironmentInitializer EnvironmentInitPrefab;
    public float EnvironmentPrefabLength = 30;
    private bool _nextSpawned;
    static EnvironmentInitializer newObj; 
    private void OnTriggerEnter(Collider other)
    {
        NewPlayerController playerController = other.GetComponentInParent<NewPlayerController>();
        if (playerController)
        {
            SpawnNext();
        }
    }
    public static void Reset()
    {
        if (newObj)
        {
            newObj.SetPosition(Vector3.zero);
        }
    }

    private void SpawnNext()
    {
        if (_nextSpawned) return;
        _nextSpawned = true;
        var newPos = GetComponentInParent<EnvironmentInitializer>().transform.position - new Vector3(EnvironmentPrefabLength, 0, 0);
        newObj = Instantiate(EnvironmentInitPrefab, newPos, Quaternion.identity);
        newObj.Spawn(this);
    }
}
