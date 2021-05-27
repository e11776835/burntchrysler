using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManagerBehaviour : MonoBehaviour
{
    public bool GameInProgress;

    public EndGameEvent OnEndGame;

    public static GameManagerBehaviour Instance;

    public void Start()
    {
        if (Instance != null)
        {
            Debug.LogError($"New instance of singleton behaviour: {nameof(GameManagerBehaviour)}. Destroying old instances gameobject.");
            if (Instance.gameObject != this.gameObject)
            {
                Destroy(Instance.gameObject);
            }
        }
        Instance = this;
        StartGame();
    }
    public void StartGame()
    {
        GameInProgress = true;
        NewPlayerController.Instance.Reset();
        EnvironmentSpawner.Reset();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("restarted");
            StartGame();
        }
    }

    internal static void EndGame(string message) => Instance.EndGame(new EndGameEventArgs(message, ScoreController.GetScore()));

    //TODO: ScoreController
    public class ScoreController { public static int GetScore() => 0; }
    private void EndGame(EndGameEventArgs args)
    {
        // for now, this does nothing
        OnEndGame.Invoke(args);
        GameInProgress = false;
        
        //Time.timeScale = 0.0f;
    }
}