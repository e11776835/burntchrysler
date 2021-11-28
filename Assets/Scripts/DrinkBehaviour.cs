using System;
using UnityEngine;

public class DrinkBehaviour : MonoBehaviour
{
    public float Drunkness = .1f;

    internal void Drink()
    {
        Debug.LogError("TO DO: drink destroy effect");
        Destroy(gameObject);
    }
}