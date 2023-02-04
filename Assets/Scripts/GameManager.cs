using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<GameObject> throwables;

    public CharacterController2D player1Controller, player2Controller;
    public TextMeshProUGUI player1Health, player2Health;
    public TextMeshProUGUI winnerTitle;
    public GameObject winnerScreen;

    List<PickableGrass> grassList;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        grassList = FindObjectsOfType<PickableGrass>().ToList<PickableGrass>();
        foreach(PickableGrass grass in grassList)
        {
            grass.containedObject = GetRandomThrowable();
        }
    }

    public GameObject GetRandomThrowable()
    {
        return throwables[Random.Range(0,throwables.Count)];
    }

    public void UpdateHealthUI(int playerNumber, int health)
    {
        if (playerNumber == 1)
        {
            player1Health.text = health.ToString();
        }
        else if (playerNumber == 2)
        {
            player2Health.text = health.ToString();
        }
        else
        {
            Debug.Log("Wrong player number");
        }
    }

    public void PlayerDied(int playerNumber)
    {
        string winner = "";
        if (playerNumber == 1)
        {
            Debug.Log("player 2 won");
            player2Controller.enabled = false;
            player2Controller.GetComponent<Rigidbody2D>().isKinematic = true;
            player2Controller.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            winner = "Rat";
        }
        else if (playerNumber == 2)
        {
            Debug.Log("player 1 won");
            player1Controller.enabled = false;
            player1Controller.GetComponent<Rigidbody2D>().isKinematic = true;
            player1Controller.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            winner = "Cat";
        }
        else
        {
            Debug.Log("Wrong player number");
        }

        winnerTitle.text = winner + " wins!";
        winnerScreen.GetComponent<Animator>().SetTrigger("GameEnd");
    }
}
