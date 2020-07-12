using System;
using UnityEngine;

internal class GameManager
{
    internal static void EndGame()
    {
        // for now, this does nothing apart from stopping time 
        Time.timeScale = 0.0f;
        Debug.LogError("End Game NYI");
    }
}