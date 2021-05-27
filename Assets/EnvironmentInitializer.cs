using System;
using System.Security.Cryptography;
using UnityEngine;

public class EnvironmentInitializer : MonoBehaviour
{
    public EnvironmentDestroyer Destroyer;
    /// <summary>
    /// Spawns next environment, and prepares last for destruction
    /// </summary>
    /// <param name="source"></param>
    public void Spawn(EnvironmentSpawner source)
    {
        Destroyer.SetObjectToDestroy(source);
    }

    internal void SetPosition(Vector3 pos)
    {
        transform.position = pos;
        Destroyer.LastEnvironment.transform.position = new Vector3(pos.x + Destroyer.LastEnvironment.EnvironmentPrefabLength, pos.y, pos.z);
    }
}