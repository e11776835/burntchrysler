using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    public EnvironmentInitializer EnvironmentInitPrefab;
    public float EnvironmentPrefabLength = 15;
    private bool _nextSpawned;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered.");
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController)
        {
            SpawnNext();
        }
    }

    private void SpawnNext()
    {
        if (_nextSpawned) return;
        _nextSpawned = true;
        var newPos = transform.position - new Vector3(EnvironmentPrefabLength, 0, 0);
        var newObj = GameObject.Instantiate(EnvironmentInitPrefab, newPos, Quaternion.identity);
        Debug.Log("Spawned next.");
    }
}
