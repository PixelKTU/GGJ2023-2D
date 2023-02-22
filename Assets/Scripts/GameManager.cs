using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Action<string> OnGameEnd;//player name
    public CharacterController2D player1Controller, player2Controller;

    private int _winner;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _winner = -1;
    }

    public void PlayerDied(int playerNumber)
    {
        if (_winner == -1)
        {
            _winner = playerNumber;
            string winner = "";
            if (playerNumber == 1)
            {
                Debug.Log("player 2 won");
                player2Controller.enabled = false;
                player2Controller.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                player2Controller.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                winner = "Rat";
            }
            else if (playerNumber == 2)
            {
                Debug.Log("player 1 won");
                player1Controller.enabled = false;
                player1Controller.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                player1Controller.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                winner = "Cat";
            }
            else
            {
                Debug.Log("Wrong player number");
            }

            OnGameEnd?.Invoke(winner);
        }
    }
}
