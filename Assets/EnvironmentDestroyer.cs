using System;
using UnityEngine;

public class EnvironmentDestroyer : MonoBehaviour
{
    EnvironmentSpawner _lastEnvironent;
    private void OnTriggerEnter(Collider other)
    {
        if (_lastEnvironent)
        {
            Destroy(_lastEnvironent);
        }
    }

    internal void SetObjectToDestroy(EnvironmentSpawner environmentInitializer)
    {
        _lastEnvironent = environmentInitializer;
    }
}
