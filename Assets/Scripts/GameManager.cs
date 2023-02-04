using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    public List<GameObject> throwables;

    public TextMeshProUGUI player1Health, player2Health;

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
        if (playerNumber == 1)
        {
            Debug.Log("player 2 won");
        }
        else if (playerNumber == 2)
        {
            Debug.Log("player 1 won");
        }
        else
        {
            Debug.Log("Wrong player number");
        }
    }
}
