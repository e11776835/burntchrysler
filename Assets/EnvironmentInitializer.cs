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
}