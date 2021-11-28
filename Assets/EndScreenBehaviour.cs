using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreenBehaviour : MonoBehaviour
{
    public GameObject EndScreenObject;
    public TMP_Text Text;
    public GameManagerBehaviour GameManager;
    private void OnEnable()
    {
        DisableEndScreen();
        GameManager.OnEndGame.AddListener(EnableEndScreen);
        GameManager.OnRequestRestart.AddListener(DisableEndScreen);
    }

    public void DisableEndScreen()
    {
        EndScreenObject.SetActive(false);
    }

    private void EnableEndScreen(EndGameEventArgs args)
    {
        Text.text = args.message;
        EndScreenObject.SetActive(true);
    }

    public void Update(){
        if(Input.GetKeyDown(KeyCode.R)){
            GameManager.Restart();
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            
            Application.Quit();
        }
    }
}

public struct EndGameEventArgs
{
    public int score;
    public string message;
    public EndGameEventArgs(string message, int score)
    {
        this.message = message;
        this.score = score;
    }
}