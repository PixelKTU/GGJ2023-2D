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

    private void Start()
    {
        for (int i = 0; i < InputManager.playerCount; i++)
        {
            InputManager.GetInputActions(i)[1].performed += TryChangeLevel;
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < InputManager.playerCount; i++)
        {
            InputManager.GetInputActions(i)[1].performed -= TryChangeLevel;
        }
    }

    public void TryChangeLevel(UnityEngine.InputSystem.InputAction.CallbackContext callback)
    {
        if (canNextMatch)
        {
            canNextMatch = false;
            NextMatch();
        }
    }
}
