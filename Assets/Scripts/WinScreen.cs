using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    private bool canNextMatch = false;


    public void EnableNextMatch()
    {
        canNextMatch = true;
    }
    public void NextMatch()
    {
        SceneManager.LoadScene(GetRandomArena());
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    int GetRandomArena()
    {
        return Random.Range(1, 4);
    }

    private void Update()
    {
        if (canNextMatch && Input.GetKeyDown(KeyCode.Space)) // Change when more inputs are added
        {
            canNextMatch = false;
            NextMatch();
        }
    }
}
