using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CharacterController2D player1Controller, player2Controller;
    public TextMeshProUGUI player1Health, player2Health;
    public TextMeshProUGUI winnerTitle;
    public GameObject winnerScreen;

    [SerializeField] private float timeUntilWinUi;

    //private List<PickableGrass> grassList;
    private int _winner;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //grassList = FindObjectsOfType<PickableGrass>().ToList<PickableGrass>();
        _winner = -1;
        //foreach (PickableGrass grass in grassList)
       // {
         //   grass.containedObject = GetRandomThrowable();
       // }
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

            winnerTitle.text = winner + " wins!";
            StartCoroutine(WaitWinUi());
        }
    }

    IEnumerator WaitWinUi()
    {
        yield return new WaitForSeconds(timeUntilWinUi);
        winnerScreen.GetComponent<Animator>().SetTrigger("GameEnd");
    }
}
