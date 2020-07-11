using UnityEngine;

public class EnvironmentInitializer : MonoBehaviour
{
    public EnvironmentDestroyer Destroyer;
    public void Spawn(EnvironmentSpawner s)
    {
        Destroyer.SetObjectToDestroy(s);
    }
}