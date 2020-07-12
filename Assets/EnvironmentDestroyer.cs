using System;
using UnityEngine;

public class EnvironmentDestroyer : MonoBehaviour
{
    EnvironmentSpawner _lastEnvironment;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered");
        if (_lastEnvironment)
        {
            Debug.Log("Destroying last environment");
            Destroy(_lastEnvironment.GetComponentInParent<EnvironmentInitializer>().gameObject);
        }
    }

    internal void SetObjectToDestroy(EnvironmentSpawner environmentInitializer)
    {
        _lastEnvironment = environmentInitializer;
    }
}
