using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManagerBehaviour : MonoBehaviour
{
    public bool GameInProgress;

    public EndGameEvent OnEndGame;
    public UnityEvent OnStartGame;
    public UnityEvent OnRequestRestart;

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
        OnStartGame.Invoke();
        Time.timeScale = 1;
        GameInProgress = true;
        BertController.Instance.Reset();
        EnvironmentSpawner.Reset();
    }

    public void Restart()
    {
        Debug.Log("restarted");
        //Hard reset : reload scene. should probably reset all variables but this is a lot easier who gives a shit
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //TODO: reset all variables
        // OnRequestRestart.Invoke();
        // StartGame();
    }

    internal static void EndGame(string message) => Instance.EndGame(new EndGameEventArgs(message, ScoreController.GetScore()));

    //TODO: ScoreController
    public class ScoreController { public static int GetScore() => 0; }
    private void EndGame(EndGameEventArgs args)
    {
        // for now, this does nothing
        OnEndGame.Invoke(args);
        GameInProgress = false;
        
        Time.timeScale = 0.0f;
    }
}