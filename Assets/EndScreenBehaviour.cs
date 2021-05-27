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
        GameManager.OnEndGame.AddListener(EnableEndScreen);
    }

    private void EnableEndScreen(EndGameEventArgs args)
    {
        EndScreenObject.SetActive(true);
        Text.text = args.message;
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