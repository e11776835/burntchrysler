using System;
using UnityEngine;

public class EnvironmentDestroyer : MonoBehaviour
{
    public EnvironmentSpawner LastEnvironment;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered");
        if (LastEnvironment)
        {
            Debug.Log("Destroying last environment");
            Destroy(LastEnvironment.GetComponentInParent<EnvironmentInitializer>().gameObject);
        }
    }

    internal void SetObjectToDestroy(EnvironmentSpawner environmentInitializer)
    {
        LastEnvironment = environmentInitializer;
    }
}
